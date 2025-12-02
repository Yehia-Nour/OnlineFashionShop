using AutoMapper;
using AutoMapper.Execution;
using ECommerce.Domain.Entities.OrderModule;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Shared.DTOs;
using ECommerce.Shared.DTOs.ProductDTOs;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Services.MappingProfiles
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDTO, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.Product.PictureUrl))
                return string.Empty;

            if (source.Product.PictureUrl.StartsWith("http"))
                return source.Product.PictureUrl;

            var baseUrl = _configuration.GetSection("URLs")["BaseUrl"];
            if (string.IsNullOrEmpty(baseUrl))
                return string.Empty;


            var picUrl = $"{baseUrl}{source.Product.PictureUrl}";
            return picUrl;
        }
    }
}