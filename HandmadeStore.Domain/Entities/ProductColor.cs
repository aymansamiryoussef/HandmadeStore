namespace HandmadeStore.Domain.Entities
{
    public class ProductColor
    {
        public string Name { get; private set; }

        private ProductColor() { }

        public ProductColor(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Color is required");

            Name = name;
        }
    }
}