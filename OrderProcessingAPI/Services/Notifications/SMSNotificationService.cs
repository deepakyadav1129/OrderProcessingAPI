using OrderProcessingAPI.Entities;
namespace OrderProcessingAPI.Services.Notifications
{
    public class SmsNotificationService : INotificationService
    {
        public Task SendOrderConfirmationAsync(Customer customer, Order order)
        {
            // Simulating an external SMS provider.
            Console.WriteLine(
                $"SMS: Order confirmation sent to {customer.MobileNumber}. " +
                $"Order Id: {order.Id}, " +
                $"Total Amount: {order.TotalAmount}");

            return Task.CompletedTask;
        }
    }
}
