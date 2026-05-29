using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabt.Reporisitory.Data.Configrations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public ProductConfiguration() { }

        public void Configure(EntityTypeBuilder<Product> builder)
        {
               builder
                .HasOne(p => p.ProductBrand)
                .WithMany()
                .HasForeignKey(p => p.ProductBrandId);
               builder
                .HasOne(p => p.ProductType)
                .WithMany()
                .HasForeignKey(p => p.ProductTypeId);
            builder
                .Property(P => P.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(P => P.Description)
                .IsRequired();

            builder
                .Property(P => P.PictureUrl)
                .IsRequired();
            builder
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}
