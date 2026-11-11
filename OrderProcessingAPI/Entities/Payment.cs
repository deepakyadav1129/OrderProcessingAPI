using OrderProcessingAPI.Enums;
namespace OrderProcessingAPI.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus Status { get; set; }

        // COD does not have a transaction id during order creation.
        public string? TransactionId { get; set; }

        // For COD, actual payment has not happened yet.
        public DateTime? PaymentDate { get; set; }
        public Order Order { get; set; } = null!;
    }
}
