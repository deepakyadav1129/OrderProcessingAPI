using OrderProcessingAPI.Enums;
namespace OrderProcessingAPI.DTOs
{
    public class OrderSummaryResponseDTO
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
    }
}
