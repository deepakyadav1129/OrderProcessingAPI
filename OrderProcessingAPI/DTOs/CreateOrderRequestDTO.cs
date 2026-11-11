using OrderProcessingAPI.Enums;
using System.ComponentModel.DataAnnotations;
namespace OrderProcessingAPI.DTOs
{
    public class CreateOrderRequestDTO
    {
        [Range(1, int.MaxValue)]
        public int CustomerId { get; set; }

        [Required]
        public string DeliveryMethod { get; set; } = string.Empty;

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        public List<CreateOrderItemRequestDTO> Items { get; set; }
            = new List<CreateOrderItemRequestDTO>();
    }
}
