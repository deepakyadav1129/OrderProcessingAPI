using OrderProcessingAPI.Entities;
namespace OrderProcessingAPI.Repositories
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment);
    }
}
