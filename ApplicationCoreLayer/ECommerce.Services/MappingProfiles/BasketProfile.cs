using AutoMapper;
using ECommerce.Domain.Entities.BasketModule;
using ECommerce.Shared.DTOs.BasketDTOs;

namespace ECommerce.Services.MappingProfiles
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<CustomerBasket, BasketDTO>().ReverseMap();

            CreateMap<BasketItem, BasketItemDTO>().ReverseMap();
        }
    }
}
