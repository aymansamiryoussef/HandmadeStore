using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; private set; }
        public string? Description { get;private set; }

        private readonly List<Product>? _products = new();
        public IReadOnlyCollection<Product>? Products => _products.AsReadOnly();

        public bool IsActive { get; set; } = true;
        public Category(string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name cannot be null or empty.", nameof(name));
            }
            Name = name;
            Description = description;
        }
        private Category() { }

        public void UpdateDetails(string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name cannot be null or empty.", nameof(name));
            }
            Name = name;
            Description = description;
        }

        public void deactivate()
        {
            IsActive = false;
        }


    }
}
