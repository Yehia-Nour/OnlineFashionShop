using ECommerce.Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Data.Configurations
{
    public class OrderConfigruation : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.SubTotal).HasPrecision(8, 2);

            builder.OwnsOne(o => o.Address, ownEntity =>
            {
                ownEntity.Property(a => a.FirstName).HasMaxLength(50);
                ownEntity.Property(a => a.LastName).HasMaxLength(50);
                ownEntity.Property(a => a.City).HasMaxLength(50);
                ownEntity.Property(a => a.Country).HasMaxLength(50);
                ownEntity.Property(a => a.Street).HasMaxLength(50);
            });
        }
    }
}
