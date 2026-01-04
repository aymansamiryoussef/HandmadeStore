using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Infrastructure.Configruation
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            // ---------- Key ----------
            builder.HasKey(c => c.Id);

            // ---------- Properties ----------
            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(c => c.Description)
                   .HasMaxLength(500);

            builder.Property(c => c.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);

            // ---------- Index ----------
            builder.HasIndex(c => c.Name)
                   .IsUnique();

            // ---------- Relationship ----------
            builder.HasMany(c => c.Products)
                   .WithOne(p => p.Category)
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
