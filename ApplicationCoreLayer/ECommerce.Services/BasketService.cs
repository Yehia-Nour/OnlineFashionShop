using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.BasketModule;
using ECommerce.Services.Exceptions;
using ECommerce.ServicesAbstraction;
using ECommerce.Shared.DTOs.BasketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<BasketDTO> GetBasketAsync(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            if (basket is null)
                throw new BasketNotFoundException(id);
            return _mapper.Map<BasketDTO>(basket);
        }

        public async Task<BasketDTO> CreateOrUpdateAsync(BasketDTO basket)
        {
            var customerBasket = _mapper.Map<CustomerBasket>(basket);
            var createOrUpdate = await _basketRepository.CreateOrUpdateBasketAsync(customerBasket);
            return _mapper.Map<BasketDTO>(createOrUpdate);
        }

        public async Task<bool> DeleteBasketAsync(string id) => await _basketRepository.DeleteBasketAsync(id);
    }
}
