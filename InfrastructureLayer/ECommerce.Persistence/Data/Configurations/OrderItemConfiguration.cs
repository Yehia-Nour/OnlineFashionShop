using ECommerce.Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Persistence.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(oi => oi.Price).HasPrecision(8, 2);

            builder.OwnsOne(oi => oi.Product, ownEnitty =>
            {
                ownEnitty.Property(p => p.ProductName).HasMaxLength(100);
                ownEnitty.Property(p => p.PictureUrl).HasMaxLength(200);
            });
        }
    }
}
