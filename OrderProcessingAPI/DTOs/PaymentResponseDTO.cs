using OrderProcessingAPI.Enums;
namespace OrderProcessingAPI.DTOs
{
    public class PaymentResponseDTO
    {
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus Status { get; set; }
        public string? TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}
