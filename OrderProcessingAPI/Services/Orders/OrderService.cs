using OrderProcessingAPI.DTOs;
using OrderProcessingAPI.Entities;
using OrderProcessingAPI.Enums;
using OrderProcessingAPI.Exceptions;
using OrderProcessingAPI.Repositories;
using OrderProcessingAPI.Services.Delivery;
using OrderProcessingAPI.Services.Discounts;
using OrderProcessingAPI.Services.Notifications;
using OrderProcessingAPI.Services.Payments;
using OrderProcessingAPI.Validators;

namespace OrderProcessingAPI.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderValidator _orderValidator;

        private readonly IEnumerable<IDiscountCalculator> _discountCalculators;
        private readonly IEnumerable<IDeliveryChargeCalculator> _deliveryChargeCalculators;
        private readonly IPaymentService _paymentService;
        private readonly IEnumerable<INotificationService> _notificationServices;

        public OrderService(
            ICustomerRepository customerRepository,
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork,
            IOrderValidator orderValidator,
            IEnumerable<IDiscountCalculator> discountCalculators,
            IEnumerable<IDeliveryChargeCalculator> deliveryChargeCalculators,
            IPaymentService paymentService,
            IEnumerable<INotificationService> notificationServices)
        {
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _orderValidator = orderValidator;
            _discountCalculators = discountCalculators;
            _deliveryChargeCalculators = deliveryChargeCalculators;
            _paymentService = paymentService;
            _notificationServices = notificationServices;
        }

        public async Task<OrderResponseDTO> CreateOrderAsync(CreateOrderRequestDTO request)
        {
            // Step 1: Validate the basic order request.
            _orderValidator.ValidateRequest(request);

            // Step 2: Find the customer.
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);

            if (customer == null)
            {
                throw new NotFoundException($"Customer with Id {request.CustomerId} was not found.");
            }

            // Step 3: Collect requested product ids.
            var productIds = request.Items
                .Select(i => i.ProductId)
                .Distinct()
                .ToList();

            // Step 4: Load products.
            var products = await _productRepository.GetByIdsAsync(productIds);

            // Step 5: Validate product existence and stock.
            _orderValidator.ValidateProducts(request, products);

            // Step 6:
            // Create order items and calculate subtotal.
            var orderItems = new List<OrderItem>();

            decimal subTotal = 0;

            foreach (var requestItem in request.Items)
            {
                var product = products.First(p => p.Id == requestItem.ProductId);

                decimal lineTotal = product.Price * requestItem.Quantity;

                subTotal += lineTotal;

                orderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Product = product,
                    Quantity = requestItem.Quantity,
                    UnitPrice = product.Price,
                    LineTotal = lineTotal
                });
            }

            // Step 7:
            // Find applicable discount calculator.
            var discountCalculator = _discountCalculators
                .FirstOrDefault(c => c.IsApplicable(customer.CustomerType));

            if (discountCalculator == null)
            {
                throw new BusinessRuleException(
                    $"No discount calculator is configured for " +
                    $"customer type '{customer.CustomerType}'.");
            }

            // Step 8: Calculate discount.
            decimal discountAmount = discountCalculator.CalculateDiscount(subTotal);

            decimal amountAfterDiscount = subTotal - discountAmount;

            // Step 9:
            // Find applicable delivery-charge calculator.
            var deliveryCalculator = _deliveryChargeCalculators
                .FirstOrDefault(c => c.IsApplicable(request.DeliveryMethod));

            if (deliveryCalculator == null)
            {
                throw new BusinessRuleException(
                    $"Delivery method '{request.DeliveryMethod}' " +
                    $"is not supported.");
            }

            // Step 10: Calculate delivery charge.
            decimal deliveryCharge = deliveryCalculator.CalculateDeliveryCharge(amountAfterDiscount);

            // Step 11: Calculate final payable amount.
            decimal totalAmount = amountAfterDiscount + deliveryCharge;

            // Step 12:
            // Create the order initially with Pending status.
            var order = new Order
            {
                CustomerId = customer.Id,
                Customer = customer,
                OrderDate = DateTime.UtcNow,
                SubTotal = subTotal,
                DiscountAmount = discountAmount,
                DeliveryCharge = deliveryCharge,
                TotalAmount = totalAmount,
                DeliveryMethod = request.DeliveryMethod.Trim(),
                PaymentMethod = request.PaymentMethod,
                Status = OrderStatus.Pending,
                Items = orderItems
            };

            // Step 13: Add the order.
            await _orderRepository.AddAsync(order);

            // Step 14:
            // Save the order first so SQL Server generates OrderId.
            await _unitOfWork.SaveChangesAsync();

            // Step 15:
            // Ask PaymentService to handle the complete payment workflow.
            Payment payment =
                await _paymentService.ProcessPaymentAsync(order.Id, totalAmount, request.PaymentMethod);

            // Keep the Payment navigation property available on the current Order object.
            order.Payment = payment;

            // Step 16:
            // Determine the final order status.
            if (request.PaymentMethod == PaymentMethod.COD)
            {
                // COD payment remains Pending because
                // money has not yet been collected.
                // However, the order itself is accepted.
                order.Status = OrderStatus.Confirmed;
            }
            else if (payment.Status == PaymentStatus.Success)
            {
                // Card/UPI payment succeeded.
                order.Status = OrderStatus.Confirmed;
            }
            else
            {
                // Online payment failed.
                order.Status = OrderStatus.Failed;
            }

            // Step 17:
            // Reduce stock only for confirmed orders.
            if (order.Status == OrderStatus.Confirmed)
            {
                foreach (var requestItem in request.Items)
                {
                    var product = products.First(p => p.Id == requestItem.ProductId);
                    product.StockQuantity -= requestItem.Quantity;
                }
            }

            // Step 18:
            // Save the Payment entity added by PaymentService,
            // the final Order status, and updated Product stock.
            await _unitOfWork.SaveChangesAsync();

            // Step 19:
            // Send confirmation only for confirmed orders.
            if (order.Status == OrderStatus.Confirmed)
            {
                foreach (var notificationService in _notificationServices)
                {
                    await notificationService.SendOrderConfirmationAsync(customer, order);
                }
            }

            // Step 20: Return the created order.
            return MapToOrderResponse(order);
        }

        public async Task<OrderResponseDTO> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                throw new NotFoundException($"Order with Id {id} was not found.");
            }

            return MapToOrderResponse(order);
        }

        public async Task<List<OrderSummaryResponseDTO>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();

            return orders
                .Select(order =>
                    new OrderSummaryResponseDTO
                    {
                        Id = order.Id,
                        CustomerName = order.Customer.Name,
                        OrderDate = order.OrderDate,
                        TotalAmount = order.TotalAmount,
                        Status = order.Status
                    })
                .ToList();
        }

        private static OrderResponseDTO MapToOrderResponse(Order order)
        {
            return new OrderResponseDTO
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer.Name,
                CustomerType = order.Customer.CustomerType,
                OrderDate = order.OrderDate,
                SubTotal = order.SubTotal,
                DiscountAmount = order.DiscountAmount,
                DeliveryCharge = order.DeliveryCharge,
                TotalAmount = order.TotalAmount,
                DeliveryMethod = order.DeliveryMethod,
                PaymentMethod = order.PaymentMethod,
                Status = order.Status,

                Items = order.Items
                    .Select(item =>
                        new OrderItemResponseDTO
                        {
                            ProductId = item.ProductId,
                            ProductName = item.Product.Name,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            LineTotal = item.LineTotal
                        })
                    .ToList(),

                Payment = order.Payment == null
                    ? null
                    : new PaymentResponseDTO
                    {
                        PaymentMethod = order.Payment.PaymentMethod,
                        Status = order.Payment.Status,
                        TransactionId = order.Payment.TransactionId,
                        Amount = order.Payment.Amount,
                        PaymentDate = order.Payment.PaymentDate
                    }
            };
        }
    }
}
