using OrderProcessingAPI.DTOs;
using OrderProcessingAPI.Entities;
using OrderProcessingAPI.Exceptions;

namespace OrderProcessingAPI.Validators
{
    public class OrderValidator : IOrderValidator
    {
        public void ValidateRequest(CreateOrderRequestDTO request)
        {
            if (request.CustomerId <= 0)
            {
                throw new BusinessRuleException("A valid customer id is required.");
            }

            if (request.Items == null || request.Items.Count == 0)
            {
                throw new BusinessRuleException("At least one order item is required.");
            }

            if (string.IsNullOrWhiteSpace(request.DeliveryMethod))
            {
                throw new BusinessRuleException("Delivery method is required.");
            }

            // Quantity and ProductId must be positive.
            foreach (var item in request.Items)
            {
                if (item.ProductId <= 0)
                {
                    throw new BusinessRuleException("Product id must be greater than zero.");
                }

                if (item.Quantity <= 0)
                {
                    throw new BusinessRuleException("Product quantity must be greater than zero.");
                }
            }

            // Do not allow the same product multiple times.
            bool hasDuplicateProducts = request.Items
                .GroupBy(i => i.ProductId)
                .Any(g => g.Count() > 1);

            if (hasDuplicateProducts)
            {
                throw new BusinessRuleException("The same product cannot be added more than once.");
            }
        }

        public void ValidateProducts(CreateOrderRequestDTO request, List<Product> products)
        {
            var requestedProductIds = request.Items
                .Select(i => i.ProductId)
                .Distinct()
                .ToList();

            var existingProductIds = products
                .Select(p => p.Id)
                .ToList();

            var missingProductIds = requestedProductIds
                .Except(existingProductIds)
                .ToList();

            if (missingProductIds.Count > 0)
            {
                throw new NotFoundException(
                    $"Product(s) not found: " +
                    $"{string.Join(", ", missingProductIds)}");
            }

            foreach (var item in request.Items)
            {
                var product = products.First(p => p.Id == item.ProductId);

                if (product.StockQuantity < item.Quantity)
                {
                    throw new BusinessRuleException(
                        $"Insufficient stock for product '{product.Name}'. " +
                        $"Available stock: {product.StockQuantity}.");
                }
            }
        }
    }
}
