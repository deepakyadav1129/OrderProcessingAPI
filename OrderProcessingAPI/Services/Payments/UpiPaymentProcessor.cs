using OrderProcessingAPI.Enums;
using OrderProcessingAPI.Models;

namespace OrderProcessingAPI.Services.Payments
{
    public class UpiPaymentProcessor : IPaymentProcessor
    {
        public bool Supports(PaymentMethod paymentMethod)
        {
            return paymentMethod == PaymentMethod.UPI;
        }

        public async Task<PaymentResult> ProcessPaymentAsync(decimal amount)
        {
            //Simulating the API Gateway
            await Task.Delay(TimeSpan.FromMilliseconds(100));

            // Simulating successful UPI payment.
            var transactionId = $"UPI-{Guid.NewGuid().ToString("N")[..12].ToUpperInvariant()}";

            var result = new PaymentResult
            {
                Status = PaymentStatus.Success,
                TransactionId = transactionId,
                Message = "UPI payment completed successfully."
            };

            return result;
        }
    }
}
