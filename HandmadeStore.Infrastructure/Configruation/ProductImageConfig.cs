using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Infrastructure.Configruation
{
    public class ProductImageConfig : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImages");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.ImageUrl)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.HasOne(i => i.Product)
                   .WithMany(p => p.Images)
                   .HasForeignKey(i => i.ProductId);
        }
    }
}
