using OrderProcessingAPI.DTOs;
namespace OrderProcessingAPI.Services.Orders
{
    public interface IOrderService
    {
        Task<OrderResponseDTO> CreateOrderAsync(CreateOrderRequestDTO request);
        Task<OrderResponseDTO> GetOrderByIdAsync(int id);
        Task<List<OrderSummaryResponseDTO>> GetAllOrdersAsync();
    }
}
