namespace ECommerce.Services.Exceptions
{
    public sealed class BasketNotFoundException(string id) : NotFoundException($" Basket With {id} Not Found");
}
