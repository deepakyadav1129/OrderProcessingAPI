using OrderProcessingAPI.Data;
using OrderProcessingAPI.Entities;

namespace OrderProcessingAPI.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly OrderProcessingDbContext _context;

        public PaymentRepository(OrderProcessingDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
        }
    }
}
