using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOs.BasketDTOs;

namespace ECommerce.ServicesAbstraction
{
    public interface IPaymentService
    {
        Task<Result<BasketDTO>> CreateOrUpdatePaymentIntentAsync(string basketId);
        Task UpdateOrderPaymentStatus(string request, string stripeSignature);
    }
}
