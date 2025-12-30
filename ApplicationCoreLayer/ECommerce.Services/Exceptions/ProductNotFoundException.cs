namespace ECommerce.Services.Exceptions
{
    public sealed class ProductNotFoundException(int id) : NotFoundException($"Product With {id} Not Found");
}
