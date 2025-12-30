using ECommerce.ServicesAbstraction;
using ECommerce.Shared.DTOs;
using ECommerce.Shared.DTOs.OrderDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Presentation.Controllers
{
    public class OrderController : ApiBaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO order)
        {
            var result = await _orderService.CreateOrderAsync(order, GetEmailFromToken());

            return HandleResult(result);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderToReturnDTO>>> GetOrders()
        {
            var result = await _orderService.GetAllOrdersAsync(GetEmailFromToken());

            return HandleResult(result);
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrder(Guid id)
        {
            var result = await _orderService.GetOrderByIdAsync(id, GetEmailFromToken());

            return HandleResult(result);
        }

        [AllowAnonymous]
        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethods()
        {
            var result = await _orderService.GetDeliveryMethodsAsync();

            return HandleResult(result);
        }
    }
}
