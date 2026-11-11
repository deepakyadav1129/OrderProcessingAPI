namespace OrderProcessingAPI.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        // One product can appear in multiple order items.
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
