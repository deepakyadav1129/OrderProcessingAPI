using Microsoft.EntityFrameworkCore;
using OrderProcessingAPI.Data;
using OrderProcessingAPI.Entities;

namespace OrderProcessingAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly OrderProcessingDbContext _context;

        public ProductRepository(OrderProcessingDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetByIdsAsync(List<int> productIds)
        {
            // Products remain tracked because stock will be modified.
            return await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();
        }
    }
}
