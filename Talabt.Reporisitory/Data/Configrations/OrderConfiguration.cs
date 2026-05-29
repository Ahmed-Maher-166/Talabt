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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(O => O.Status)
                   .HasConversion(
                       s => s.ToString(),
                       s => (OrderStatus)Enum.Parse(typeof(OrderStatus), s)
                   );

            builder.Property(O => O.Subtotal)
                   .HasColumnType("decimal(18,2)");

            builder.OwnsOne(O => O.ShippingAddress, X =>
            {
                X.WithOwner();
            });

            builder.HasMany(O => O.Items)
                   .WithOne()
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
