using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Application.DTOs.Product
{
    public class GetProductDto
    {
        public int Id { get; set; }

        // Basic Info
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        // Pricing
        public decimal Price { get; set; }
        public bool IsSoldOut { get; set; }

        // Media
        public List<string> ImageUrls { get; set; } = new();

        // Colors
        public List<string> Colors { get; set; } = new();

    }
}
