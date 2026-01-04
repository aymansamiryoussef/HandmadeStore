namespace HandmadeStore.Domain.Entities
{
    // Domain/Entities/OrderItem.cs
    public class OrderItem
    {
        public int Id { get; private set; }
        public int OrderId { get; private set; }
        public Order Order { get; private set; } = null!;

        public int ProductId { get; private set; }
        public string ProductName { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }
        public decimal TotalPrice { get; private set; }

        // For EF Core
        private OrderItem() { }

        public OrderItem(int productId, string productName, decimal price, int quantity)
        {
            if (quantity <= 0)
                throw new Exception("Quantity must be greater than zero");

            if (price < 0)
                throw new Exception("Price cannot be negative");

            ProductId = productId;
            ProductName = productName;
            Price = price;
            Quantity = quantity;
            TotalPrice = price * quantity;
        }
    }
}