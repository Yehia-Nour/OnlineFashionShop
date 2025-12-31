using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOs.OrderDTOs;

namespace ECommerce.ServicesAbstraction
{
    public interface IOrderService
    {
        Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string email);
        Task<Result<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethodsAsync();
        Task<Result<IEnumerable<OrderToReturnDTO>>> GetAllOrdersAsync(string email);
        Task<Result<OrderToReturnDTO>> GetOrderByIdAsync(Guid id, string email);
    }
}
