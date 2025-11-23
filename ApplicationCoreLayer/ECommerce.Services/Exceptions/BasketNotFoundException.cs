using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Exceptions
{
    public sealed class BasketNotFoundException(string id) : NotFoundException($" Basket With {id} Not Found");
}
