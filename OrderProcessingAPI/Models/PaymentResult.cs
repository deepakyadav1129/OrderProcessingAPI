using OrderProcessingAPI.Enums;
namespace OrderProcessingAPI.Models
{
    public class PaymentResult
    {
        public PaymentStatus Status { get; set; }
        public string? TransactionId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
