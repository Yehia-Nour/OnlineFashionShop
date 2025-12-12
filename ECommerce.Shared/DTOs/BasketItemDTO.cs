using System.ComponentModel.DataAnnotations;

namespace ECommerce.Shared.DTOs
{
    public class BasketItemDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }

        [Range(1, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; }
    }
}
