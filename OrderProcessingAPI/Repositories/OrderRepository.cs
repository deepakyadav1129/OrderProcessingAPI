using Microsoft.EntityFrameworkCore;
using OrderProcessingAPI.Data;
using OrderProcessingAPI.Entities;

namespace OrderProcessingAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderProcessingDbContext _context;

        public OrderRepository(OrderProcessingDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.Customer)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.Payment)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.Customer)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
    }
}
