namespace ECommerce.Domain.Entities.ProductModule
{
    public class ProductType : BaseEntity<int>
    {
        public string Name { get; set; } = default!;
    }
}
