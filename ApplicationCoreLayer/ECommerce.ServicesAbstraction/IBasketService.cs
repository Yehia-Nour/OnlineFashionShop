using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOs.BasketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServicesAbstraction
{
    public interface IBasketService
    {
        Task<Result<BasketDTO>> GetBasketAsync(string id);
        Task<BasketDTO> CreateOrUpdateAsync(BasketDTO basket);
        Task<bool> DeleteBasketAsync(string id);
    }
}
