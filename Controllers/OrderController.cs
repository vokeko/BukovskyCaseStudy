using BukovskyCaseStudy.Models;
using BukovskyCaseStudy.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BukovskyCaseStudy.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController(OrderService orderService) : ControllerBase
    {
        private readonly OrderService _orderService = orderService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrderList()
        {
            var orders = await _orderService.GetOrderListAsync();
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order)
        {
            var createdOrder = await _orderService.CreateOrderAsync(order);
            return Ok(createdOrder);
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> ProcessOrder([Required] Guid id, [FromQuery] bool isPaid)
        {
            try
            {
                var processedOrder = await _orderService.ProcessOrderAsync(id, isPaid);
                if (processedOrder == null) return NotFound();
                return Ok(processedOrder);
            }
            catch (InvalidOperationException ex)
            {
                return Forbid(ex.Message);
            }
        }
    }
}
