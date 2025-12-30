using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.BasketModule;
using ECommerce.Domain.Entities.OrderModule;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Services.Specifications.OrderSpecifications;
using ECommerce.ServicesAbstraction;
using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOs;
using ECommerce.Shared.DTOs.OrderDTOs;

namespace ECommerce.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IBasketRepository basketRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        public async Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string email)
        {
            var orderAddress = _mapper.Map<OrderAddress>(orderDTO.Address);

            var basket = await _basketRepository.GetBasketAsync(orderDTO.BasketId);
            if (basket is null)
                return Error.NotFound("Basket.NotFound", $"The Basket With Id {orderDTO.BasketId} Is Not Found");

            List<OrderItem> orderItems = [];

            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id);
                if (product is null)
                    return Error.NotFound("Product.NotFound", $"The Product With Id {item.Id} Is Not Found");
                orderItems.Add(CreateOrderItem(item, product));
            }

            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDTO.DeliveryMethodId);
            if (deliveryMethod is null)
                return Error.NotFound("DeliveryMethod.NotFound", $"The DeliveryMethod With Id {orderDTO.DeliveryMethodId} Is Not Found");

            var subTotal = orderItems.Sum(oi => oi.Price * oi.Quantity);

            var order = new Order
            {
                Address = orderAddress,
                DeliveryMethod = deliveryMethod,
                Items = orderItems,
                SubTotal = subTotal,
                UserEmail = email
            };

            await _unitOfWork.GetRepository<Order, Guid>().AddAsync(order);
            int result = await _unitOfWork.SaveChangesAsync();
            if (result == 0)
                return Error.Failure("Order.Failure", $"Order Can't Be Created");

            return _mapper.Map<OrderToReturnDTO>(order);
        }

        public async Task<Result<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();

            return _mapper.Map<IEnumerable<DeliveryMethodDTO>>(deliveryMethods).ToList();
        }

        public async Task<Result<IEnumerable<OrderToReturnDTO>>> GetAllOrdersAsync(string email)
        {
            var orderSpec = new OrderSpecification(email);
            var orders = await _unitOfWork.GetRepository<Order, Guid>().GetAllAsync(orderSpec);

            return _mapper.Map<IEnumerable<OrderToReturnDTO>>(orders).ToList();
        }

        public async Task<Result<OrderToReturnDTO>> GetOrderByIdAsync(Guid id, string email)
        {
            var orderSpec = new OrderSpecification(id, email);
            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(orderSpec);
            if (order is null)
                return Error.NotFound("Order.NotFound", $"Order With Id: {id} Not Found");

            return _mapper.Map<OrderToReturnDTO>(order);
        }

        private static OrderItem CreateOrderItem(BasketItem item, Product product)
        {
            return new OrderItem
            {
                Product = new ProductItemOrdered { ProductId = product.Id, PictureUrl = product.PictureUrl, ProductName = product.Name },
                Price = product.Price,
                Quantity = item.Quantity
            };
        }
    }
}
