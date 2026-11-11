using Microsoft.EntityFrameworkCore;
using OrderProcessingAPI.Data;
using OrderProcessingAPI.Entities;

namespace OrderProcessingAPI.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly OrderProcessingDbContext _context;

        public CustomerRepository(OrderProcessingDbContext context)
        {
            _context = context;
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
