namespace ECommerce.Shared.DTOs.BasketDTOs
{
    public record BasketDTO(string Id, ICollection<BasketItemDTO> Items);
}
