using OrderProcessingAPI.Entities;
using OrderProcessingAPI.Enums;
using OrderProcessingAPI.Exceptions;
using OrderProcessingAPI.Models;
using OrderProcessingAPI.Repositories;

namespace OrderProcessingAPI.Services.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly IEnumerable<IPaymentProcessor> _paymentProcessors;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(
            IEnumerable<IPaymentProcessor> paymentProcessors,
            IPaymentRepository paymentRepository,
            IUnitOfWork unitOfWork)
        {
            _paymentProcessors = paymentProcessors;
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Payment> ProcessPaymentAsync(int orderId, decimal amount, PaymentMethod paymentMethod)
        {
            // Find the payment processor that supports the requested payment method.
            var paymentProcessor = _paymentProcessors
                .FirstOrDefault(p => p.Supports(paymentMethod));

            if (paymentProcessor == null)
            {
                throw new BusinessRuleException($"Payment method '{paymentMethod}' is not supported.");
            }

            // Process the payment using the selected processor.
            PaymentResult paymentResult = await paymentProcessor.ProcessPaymentAsync(amount);

            // Create the Payment entity from the result returned by the payment processor.
            var payment = new Payment
            {
                OrderId = orderId,
                Amount = amount,
                PaymentMethod = paymentMethod,
                Status = paymentResult.Status,
                TransactionId = paymentResult.TransactionId,

                // PaymentDate exists only when money
                // has actually been collected successfully.
                PaymentDate =
                    paymentResult.Status == PaymentStatus.Success
                        ? DateTime.UtcNow
                        : null
            };

            // Add the payment to the DbContext through
            // the payment repository.
            // SaveChanges will be called by UnitOfWork
            // from the coordinating order workflow.
            await _paymentRepository.AddAsync(payment);
            await _unitOfWork.SaveChangesAsync();
            return payment;
        }
    }
}
