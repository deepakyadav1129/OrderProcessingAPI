using OrderProcessingAPI.Entities;
namespace OrderProcessingAPI.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetByIdsAsync(List<int> productIds);
    }
}
