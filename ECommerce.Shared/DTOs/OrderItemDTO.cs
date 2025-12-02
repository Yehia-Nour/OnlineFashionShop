namespace ECommerce.Shared.DTOs
{
    public record OrderItemDTO(string ProductName, string PictureUrl, decimal Price, int Quantity);
}