using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOs.BasketDTOs;

namespace ECommerce.ServicesAbstraction
{
    public interface IBasketService
    {
        Task<Result<BasketDTO>> GetBasketAsync(string id);
        Task<Result<BasketDTO>> CreateOrUpdateAsync(BasketDTO basket);
        Task<Result<bool>> DeleteBasketAsync(string id);
    }
}
