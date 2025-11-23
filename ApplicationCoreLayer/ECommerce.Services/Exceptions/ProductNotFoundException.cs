using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Exceptions
{
    public sealed class ProductNotFoundException(int id) : NotFoundException($"Product With {id} Not Found");
}
