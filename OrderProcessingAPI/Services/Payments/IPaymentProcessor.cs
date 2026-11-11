using OrderProcessingAPI.Enums;
using OrderProcessingAPI.Models;
namespace OrderProcessingAPI.Services.Payments
{
    public interface IPaymentProcessor
    {
        bool Supports(PaymentMethod paymentMethod);
        Task<PaymentResult> ProcessPaymentAsync(decimal amount);
    }
}

