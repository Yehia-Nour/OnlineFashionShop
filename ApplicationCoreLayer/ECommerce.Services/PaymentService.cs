using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.OrderModule;
using ECommerce.Services.Specifications.OrderSpecifications;
using ECommerce.ServicesAbstraction;
using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOs.BasketDTOs;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace ECommerce.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PaymentService(
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,
            IConfiguration configuration,
            IMapper mapper
        )
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<Result<BasketDTO>> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            var skey = _configuration["Stripe:Skey"];
            if (skey is null)
                return Error.Failure("Failed to obtain Secret Ket Value");
            StripeConfiguration.ApiKey = skey;
            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket is null)
                return Error.NotFound("Basket not found");


            if (basket.DeliveryMethodId is null)
                return Error.Validation("Delivery method is not selected in the basket");


            var method = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value);

            if (method is null)
                return Error.NotFound("Delivery method not found");

            basket.ShippingPrice = method.Price;

            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.GetRepository<Domain.Entities.ProductModule.Product, int>().GetByIdAsync(item.Id);

                if (product is null)
                    return Error.NotFound("ProductItem.NotFound");

                item.Price = product.Price;
                item.ProductName = product.Name;
                item.PictureUrl = product.PictureUrl;
            }

            long amount = (long)(basket.Items.Sum(I => I.Quantity * I.Price) * 100);


            var stripeService = new PaymentIntentService();
            if (basket.PaymentIntentID is null)
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = amount,
                    Currency = "USD",
                    PaymentMethodTypes = ["card"],
                };
                var paymentIntent = await stripeService.CreateAsync(options);

                basket.PaymentIntentID = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions { Amount = amount };

                await stripeService.UpdateAsync(basket.PaymentIntentID, options);
            }

            await _basketRepository.CreateOrUpdateBasketAsync(basket);

            return _mapper.Map<BasketDTO>(basket);
        }

        public async Task UpdateOrderPaymentStatus(string request, string stripeSignature)
        {
            var endPointSecret = _configuration["Stripe:EndpointSecret"];
            var stripeEvent = EventUtility.ConstructEvent(request, stripeSignature, endPointSecret);

            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            var order = await _unitOfWork
                .GetRepository<Order, Guid>()
                .GetByIdAsync(new OrderWithPaymentIntentSpecifications(paymentIntent!.Id));
            if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
            {
                order.OrderStatus = OrderStatus.PaymentRecvied;

                _unitOfWork.GetRepository<Order, Guid>().Update(order);

                await _unitOfWork.SaveChangesAsync();
            }
            else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
            {
                order.OrderStatus = OrderStatus.PaymentFailed;
                _unitOfWork.GetRepository<Order, Guid>().Update(order);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
            }
        }
    }
}
