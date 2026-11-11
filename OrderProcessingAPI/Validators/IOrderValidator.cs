using OrderProcessingAPI.DTOs;
using OrderProcessingAPI.Entities;

namespace OrderProcessingAPI.Validators
{
    public interface IOrderValidator
    {
        void ValidateRequest(CreateOrderRequestDTO request);
        void ValidateProducts(CreateOrderRequestDTO request, List<Product> products);
    }
}
