using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HandmadeStore.Infrastructure.Configruation
{
    // Infrastructure/Data/Configurations/OrderItemConfiguration.cs
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.ProductName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(oi => oi.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(oi => oi.TotalPrice)
                .HasColumnType("decimal(18,2)");
        }
    }
}
