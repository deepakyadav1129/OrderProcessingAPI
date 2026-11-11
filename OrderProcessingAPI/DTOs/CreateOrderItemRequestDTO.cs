using System.ComponentModel.DataAnnotations;
namespace OrderProcessingAPI.DTOs
{
    public class CreateOrderItemRequestDTO
    {
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
