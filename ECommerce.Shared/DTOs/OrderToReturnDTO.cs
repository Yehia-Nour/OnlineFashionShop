using ECommerce.Shared.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.DTOs
{
    public record OrderToReturnDTO(Guid Id,
        string UserEmail,
        ICollection<OrderItemDTO> Items,
        AddressDTO Address,
        string DeliveryMethod,
        string OrderStatus,
        DateTimeOffset OrderDate,
        decimal SubTotal,
        decimal Total);
}
