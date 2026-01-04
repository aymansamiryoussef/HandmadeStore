using HandmadeStore.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Domain.Entities
{
    public class Cart
    {
        public int Id { get; set; }

        public string UserId { get; set; }   
       

        public CartStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<CartItem> _items { get; set; } = new List<CartItem>();
        private Cart() { }

        public Cart(string userId)
        {
            UserId = userId;
            Status = CartStatus.Active;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddItem(int productId, decimal price, int quantity)
        {
            if (quantity <= 0)
                throw new Exception("Quantity must be greater than zero");

            var item = _items.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
                _items.Add(new CartItem(productId, price, quantity));
            else
                item.Increase(quantity);
        }

        public void RemoveItem(int productId)
        {
            var item = _items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
                throw new Exception("Item not found");

            _items.Remove(item);
        }

        public void ChangeQuantity(int productId, int quantity)
        {
            if (quantity <= 0)
                throw new Exception("Invalid quantity");

            var item = _items.First(i => i.ProductId == productId);
            item.SetQuantity(quantity);
        }

        public void Clear()
        {
            _items.Clear();
        }



    }
}
