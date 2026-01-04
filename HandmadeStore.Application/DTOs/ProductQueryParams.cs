using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Application.DTOs
{
    public class ProductQueryParams
    {
        public bool InStockOnly { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public string? Sort { get; set; }

    }
}
