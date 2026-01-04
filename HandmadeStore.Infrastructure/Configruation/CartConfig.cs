    // Infrastructure/Data/Configurations/CartConfiguration.cs
    using HandmadeStore.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    namespace HandmadeStore.Infrastructure.Data.Configurations
    {
        public class CartConfiguration : IEntityTypeConfiguration<Cart>
        {
            public void Configure(EntityTypeBuilder<Cart> builder)
            {
                builder.HasKey(c => c.Id);

                builder.Property(c => c.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                builder.Property(c => c.Status)
                    .IsRequired()
                    .HasConversion<int>();

                builder.Property(c => c.CreatedAt)
                    .IsRequired();

                builder.Property(c => c.UpdatedAt)
                    .IsRequired();

                builder.HasMany(c => c._items)
                    .WithOne(i => i.Cart)
                    .HasForeignKey(i => i.CartId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasIndex(c => c.UserId);
            }
        }
    }