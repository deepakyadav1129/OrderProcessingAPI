using OrderProcessingAPI.Entities;
namespace OrderProcessingAPI.Services.Notifications
{
    public class EmailNotificationService : INotificationService
    {
        public Task SendOrderConfirmationAsync(Customer customer, Order order)
        {
            // Simulating an external email provider.
            Console.WriteLine(
                $"EMAIL: Order confirmation sent to {customer.Email}. " +
                $"Order Id: {order.Id}, " +
                $"Total Amount: {order.TotalAmount}");

            return Task.CompletedTask;
        }
    }
}
