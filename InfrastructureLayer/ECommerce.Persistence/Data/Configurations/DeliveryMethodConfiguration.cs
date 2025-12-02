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
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(dm => dm.Price).HasPrecision(8, 2);

            builder.Property(dm => dm.ShortName).HasMaxLength(50);
            builder.Property(dm => dm.DeliveryTime).HasMaxLength(50);
            builder.Property(dm => dm.Description).HasMaxLength(100);
        }
    }
}
