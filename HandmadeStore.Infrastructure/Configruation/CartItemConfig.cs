using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Infrastructure.Configruation
{
    public class CartItemConfig : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.ProductId)
                .IsRequired();

            builder.Property(ci => ci.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(ci => ci.Quantity)
                .IsRequired();

            builder.HasIndex(ci => new { ci.CartId, ci.ProductId });
        }

    }
}
