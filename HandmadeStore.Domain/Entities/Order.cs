using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Domain.Entities
{
    // Domain/Entities/Order.cs
    public class Order
    {
        public int Id { get; private set; }
        public string UserId { get; private set; }
        public string OrderNumber { get; private set; }

        // Shipping Information
        public string ShippingAddress { get; private set; }
        public string ShippingCity { get; private set; }
        public string ShippingCountry { get; private set; }
        public string ShippingZipCode { get; private set; }
        public string PhoneNumber { get; private set; }

        // Order Details
        public decimal SubTotal { get; private set; }
        public decimal ShippingCost { get; private set; }
        public decimal Tax { get; private set; }
        public decimal Total { get; private set; }

        public OrderStatus Status { get; private set; }
        public PaymentStatus PaymentStatus { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        private readonly List<OrderItem> _items = new List<OrderItem>();
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        // For EF Core
        private Order() { }

        public Order(
            string userId,
            string shippingAddress,
            string shippingCity,
            string shippingCountry,
            string shippingZipCode,
            string phoneNumber)
        {
            UserId = userId;
            OrderNumber = GenerateOrderNumber();
            ShippingAddress = shippingAddress;
            ShippingCity = shippingCity;
            ShippingCountry = shippingCountry;
            ShippingZipCode = shippingZipCode;
            PhoneNumber = phoneNumber;
            Status = OrderStatus.Pending;
            PaymentStatus = PaymentStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        public void AddItem(int productId, string productName, decimal price, int quantity)
        {
            if (Status != OrderStatus.Pending)
                throw new Exception("Cannot modify order that is not pending");

            var item = new OrderItem(productId, productName, price, quantity);
            _items.Add(item);
            CalculateTotals();
        }

        public void AddItemsFromCart(IEnumerable<CartItem> cartItems, Dictionary<int, string> productNames)
        {
            foreach (var cartItem in cartItems)
            {
                var productName = productNames.GetValueOrDefault(cartItem.ProductId, "Unknown Product");
                AddItem(cartItem.ProductId, productName, cartItem.Price, cartItem.Quantity);
            }
        }

        public void SetShippingCost(decimal shippingCost)
        {
            ShippingCost = shippingCost;
            CalculateTotals();
        }

        public void SetTax(decimal tax)
        {
            Tax = tax;
            CalculateTotals();
        }

        private void CalculateTotals()
        {
            SubTotal = _items.Sum(i => i.TotalPrice);
            Total = SubTotal + ShippingCost + Tax;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ConfirmOrder()
        {
            if (Status != OrderStatus.Pending)
                throw new Exception("Order already confirmed");

            Status = OrderStatus.Confirmed;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsProcessing()
        {
            if (Status != OrderStatus.Confirmed)
                throw new Exception("Order must be confirmed first");

            Status = OrderStatus.Processing;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsShipped()
        {
            if (Status != OrderStatus.Processing)
                throw new Exception("Order must be processing first");

            Status = OrderStatus.Shipped;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsDelivered()
        {
            if (Status != OrderStatus.Shipped)
                throw new Exception("Order must be shipped first");

            Status = OrderStatus.Delivered;
            CompletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void CancelOrder()
        {
            if (Status == OrderStatus.Delivered || Status == OrderStatus.Cancelled)
                throw new Exception("Cannot cancel delivered or already cancelled order");

            Status = OrderStatus.Cancelled;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkPaymentAsCompleted()
        {
            PaymentStatus = PaymentStatus.Completed;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkPaymentAsFailed()
        {
            PaymentStatus = PaymentStatus.Failed;
            UpdatedAt = DateTime.UtcNow;
        }

        private string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper()}";
        }
    }
}
