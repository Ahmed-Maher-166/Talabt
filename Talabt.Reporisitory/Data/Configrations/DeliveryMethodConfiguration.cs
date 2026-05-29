using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Order;

namespace Talabt.Reporisitory.Data.Configrations
{
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(dm => dm.Cost)
                   .HasColumnType("decimal(18,2)");

            builder.Property(dm => dm.ShortName)
                   .IsRequired();

            builder.Property(dm => dm.Description)
                   .IsRequired();
        }
    }
}
