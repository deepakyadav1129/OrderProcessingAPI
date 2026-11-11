using OrderProcessingAPI.Enums;
using OrderProcessingAPI.Models;

namespace OrderProcessingAPI.Services.Payments
{
    public class CodPaymentProcessor : IPaymentProcessor
    {
        public bool Supports(PaymentMethod paymentMethod)
        {
            return paymentMethod == PaymentMethod.COD;
        }

        public Task<PaymentResult> ProcessPaymentAsync(decimal amount)
        {
            // COD does not collect money during order creation.
            // Therefore, payment remains Pending.
            var result = new PaymentResult
            {
                Status = PaymentStatus.Pending,
                TransactionId = null,
                Message = "Cash on Delivery order accepted. Payment is pending."
            };

            return Task.FromResult(result);
        }
    }
}
