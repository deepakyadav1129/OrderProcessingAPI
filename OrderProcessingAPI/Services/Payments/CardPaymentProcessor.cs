using OrderProcessingAPI.Enums;
using OrderProcessingAPI.Models;
namespace OrderProcessingAPI.Services.Payments
{
    public class CardPaymentProcessor : IPaymentProcessor
    {
        public bool Supports(PaymentMethod paymentMethod)
        {
            return paymentMethod == PaymentMethod.Card;
        }

        public async Task<PaymentResult> ProcessPaymentAsync(decimal amount)
        {
            //Simulating the API Gateway
            await Task.Delay(TimeSpan.FromMilliseconds(100));

            // Simulating successful Card payment.
            var transactionId = $"CARD-{Guid.NewGuid().ToString("N")[..12].ToUpperInvariant()}";

            var result = new PaymentResult
            {
                Status = PaymentStatus.Success,
                TransactionId = transactionId,
                Message = "Card payment completed successfully."
            };

            return result;
        }
    }
}
