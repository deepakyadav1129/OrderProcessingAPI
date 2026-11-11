using OrderProcessingAPI.Enums;
namespace OrderProcessingAPI.DTOs
{
    public class OrderResponseDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public CustomerType CustomerType { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal TotalAmount { get; set; }
        public string DeliveryMethod { get; set; } = string.Empty;
        public PaymentMethod PaymentMethod { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItemResponseDTO> Items { get; set; } = new List<OrderItemResponseDTO>();
        public PaymentResponseDTO? Payment { get; set; }
    }
}
