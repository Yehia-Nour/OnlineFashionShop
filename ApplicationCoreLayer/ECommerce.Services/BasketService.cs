using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.BasketModule;
using ECommerce.ServicesAbstraction;
using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOs.BasketDTOs;

namespace ECommerce.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        public async Task<Result<BasketDTO>> GetBasketAsync(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            if (basket is null)
                return Error.NotFound("Product.NotFound", $"Product With Id: {id} Not Found");

            return _mapper.Map<BasketDTO>(basket);
        }

        public async Task<Result<BasketDTO>> CreateOrUpdateAsync(BasketDTO basket)
        {
            var customerBasket = _mapper.Map<CustomerBasket>(basket);
            var createOrUpdate = await _basketRepository.CreateOrUpdateBasketAsync(customerBasket);
            return _mapper.Map<BasketDTO>(createOrUpdate);
        }

        public async Task<Result<bool>> DeleteBasketAsync(string id) => await _basketRepository.DeleteBasketAsync(id);
    }
}
