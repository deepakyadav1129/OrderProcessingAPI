using OrderProcessingAPI.Entities;
using OrderProcessingAPI.Enums;
namespace OrderProcessingAPI.Services.Payments
{
    public interface IPaymentService
    {
        Task<Payment> ProcessPaymentAsync(int orderId, decimal amount, PaymentMethod paymentMethod);
    }
}
