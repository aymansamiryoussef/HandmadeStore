using HandmadeStore.Application.DTOs;
using HandmadeStore.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Application.Specifications
{
    public class AvailableProductsSpec: BaseSpecification<Product>
    {
        public AvailableProductsSpec(ProductQueryParams query) 
            
        {

            // In Stock
            if (query.InStockOnly)
                AddCriteria(product => product.StockQuantity > 0);

            // Price Range
            if (query.MinPrice.HasValue)
                AddCriteria(product => product.Price >= query.MinPrice.Value);

            if (query.MaxPrice.HasValue)
                AddCriteria(product => product.Price <= query.MaxPrice.Value);


            // 3️⃣ Includes
            AddInclude(p => p.Images);

            // 4️⃣ Sorting
            switch (query.Sort)
            {
                case "priceAsc":
                    AddOrderBy(p => p.Price);
                    break;

                case "priceDesc":
                    AddOrderByDescending(p => p.Price);
                    break;

                case "nameAsc":
                    AddOrderBy(p => p.Name);
                    break;

                case "nameDesc":
                    AddOrderByDescending(p => p.Name);
                    break;
                case "DateOldtoNew":
                    AddOrderByDescending(p => p.CreatedAt);
                    break;
                case "DateNewtoOld":
                    AddOrderByDescending(p => p.CreatedAt);
                    break;

                default:
                    AddOrderBy(p => p.Name);
                    break;
            }

        }

    }
}
