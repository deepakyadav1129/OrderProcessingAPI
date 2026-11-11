using OrderProcessingAPI.Data;
namespace OrderProcessingAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrderProcessingDbContext _context;

        public UnitOfWork(OrderProcessingDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
