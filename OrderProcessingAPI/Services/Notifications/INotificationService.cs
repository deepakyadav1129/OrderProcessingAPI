using OrderProcessingAPI.Entities;
namespace OrderProcessingAPI.Services.Notifications
{
    public interface INotificationService
    {
        Task SendOrderConfirmationAsync(Customer customer, Order order);
    }
}

