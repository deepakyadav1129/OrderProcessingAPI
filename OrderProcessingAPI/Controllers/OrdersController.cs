using Microsoft.AspNetCore.Mvc;
using OrderProcessingAPI.DTOs;
using OrderProcessingAPI.Services.Orders;

namespace OrderProcessingAPI.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // POST: api/orders
        [HttpPost]
        public async Task<ActionResult<OrderResponseDTO>> CreateOrder(CreateOrderRequestDTO request)
        {
            // Controller delegates the business workflow to IOrderService.
            var order = await _orderService.CreateOrderAsync(request);

            return CreatedAtAction(
                nameof(GetOrderById),
                new { id = order.Id },
                order);
        }

        // GET: api/orders/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderResponseDTO>> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            return Ok(order);
        }

        // GET: api/orders
        [HttpGet]
        public async Task<ActionResult<List<OrderSummaryResponseDTO>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }
    }
}
