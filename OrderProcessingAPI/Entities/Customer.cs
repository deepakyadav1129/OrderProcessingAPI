using OrderProcessingAPI.Enums;
namespace OrderProcessingAPI.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public CustomerType CustomerType { get; set; }
        // One customer can have multiple orders.
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
