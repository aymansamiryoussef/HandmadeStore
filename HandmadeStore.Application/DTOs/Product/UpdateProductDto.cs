using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Application.DTOs.Product
{
    public class UpdateProductDto
    {
        
        // Basic Info
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        // Pricing & Stock
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        // New Images (optional)
        public List<string>? Images { get; set; }

        // Colors
        public List<string>? Colors { get; set; } = new();

    }
}
