using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.DTOs.OrderDTOs
{
    public record DeliveryMethodDTO(int Id, string ShortName, string Desciption, string DeliveryTime, decimal Price);
}
