using OrderProcessingAPI.Enums;
namespace OrderProcessingAPI.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal TotalAmount { get; set; }
        public string DeliveryMethod { get; set; } = string.Empty;
        public PaymentMethod PaymentMethod { get; set; }
        public OrderStatus Status { get; set; }

        // Navigation property.
        public Customer Customer { get; set; } = null!;

        // One order can contain multiple items.
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        // One order has one payment.
        public Payment? Payment { get; set; }
    }
}
