using OrderProcessingAPI.Entities;
namespace OrderProcessingAPI.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(int id);
    }
}
