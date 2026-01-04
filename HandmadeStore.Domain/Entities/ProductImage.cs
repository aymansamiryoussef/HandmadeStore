using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Domain.Entities
{
    public class ProductImage
    {
        public int Id { get; private set; }
        public string ImageUrl { get; private set; }


        public int ProductId { get; private set; }
        public Product Product { get; private set; }

        private ProductImage() { }
        public ProductImage(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new Exception("Invalid image url");

            ImageUrl = imageUrl;
        }
    }
}
