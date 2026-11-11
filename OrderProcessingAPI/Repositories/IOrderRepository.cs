using OrderProcessingAPI.Entities;
namespace OrderProcessingAPI.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<Order?> GetByIdAsync(int id);
        Task<List<Order>> GetAllAsync();
    }
}
