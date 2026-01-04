using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Domain.Entities
{
    public class CartItem
    {
        public int Id { get; private set; }
        public int ProductId { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }

        public int CartId { get; private set; }
        public Cart Cart { get; private set; }=null!;


        public CartItem(int productId, decimal price, int quantity)
        {
            ProductId = productId;
            Price = price;
            Quantity = quantity;
        }

        public void Increase(int quantity)
        {
            Quantity += quantity;
        }

        public void SetQuantity(int quantity)
        {
            Quantity = quantity;
        }

    }
}
