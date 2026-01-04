using System;
using System.Collections.Generic;
using System.Text;



namespace HandmadeStore.Application.DTOs.Product
{
    public class CreateProductDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public List<string> ImageUrls { get; set; } = new();
        public List<string> Colors { get; set; } = new();



    }
}
