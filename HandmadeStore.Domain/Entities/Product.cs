using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Domain.Entities
{
    public class Product
    {
        public int Id { get; private set; }

        public string Name { get; private set; }
        public string Description { get; private set; }

        public decimal Price { get; private set; }
        public int StockQuantity { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public string? MainImageUrl { get; private set; }
        private readonly List<ProductImage> _images = new();
        public IReadOnlyCollection<ProductImage> Images => _images.AsReadOnly();

        private readonly List<ProductColor> _colors = new();
        public IReadOnlyCollection<ProductColor> Colors => _colors.AsReadOnly();
        
        public int? CategoryId { get; private set; }
        public Category Category { get; private set; }

        public bool IsSoldOut => StockQuantity == 0;

        public bool IsDeleted { get; private set; } = false;

        private Product() { }

        public Product(string name, string description, decimal price, int stockQuantity, IEnumerable<string> images,
        IEnumerable<string> colors)
        {
            Name = name;
            Description = description;
            Price = price;
            StockQuantity = stockQuantity;
            CreatedAt = DateTime.UtcNow;
            SetImages(images);
            SetColors(colors);

        }
        public void UpdateProduct(string name, string description, decimal price, int stockQuantity, IEnumerable<string> images,
        IEnumerable<string> colors)
        {
            Name = name;
            Description = description;
            Price = price;
            StockQuantity = stockQuantity;
            SetImages(images);
            SetColors(colors);
        }
        public void MarkAsDeleted()
        {
            IsDeleted = true;

        }
        private void SetImages(IEnumerable<string> images)
        {
            _images.Clear();
            _images.AddRange(images.Select(i => new ProductImage(i)));
        }

        private void SetColors(IEnumerable<string> colors)
        {
            _colors.Clear();
            _colors.AddRange(colors.Select(c => new ProductColor(c)));
        }

    }
}
