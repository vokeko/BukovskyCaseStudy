using BukovskyCaseStudy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BukovskyCaseStudy.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {

        private readonly OrderDbContext _dbContext;
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger, OrderDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [Route("")]
        [HttpGet(Name = "GetOrderList")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrderList()
        {
            return await _dbContext.Orders.ToListAsync();
        }

        [Route("")]
        [HttpPost(Name = "CreateOrder")]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order)
        {
            order.DateCreated = DateTime.UtcNow;
            _dbContext.Orders.Add(order);

            if (order.OrderItems.Any())
                _dbContext.OrderItems.AddRange(order.OrderItems.Select(i => { i.OrderId = order.Id; return i; }).ToList());

            await _dbContext.SaveChangesAsync();

            return Ok(order);
        }

        [Route("{id:guid}")]
        [HttpPatch(Name = "ProcessOrder")]
        public async Task<IActionResult> ProcessOrder([FromRoute] Guid id, [FromQuery] bool isPaid)
        {
            var order = GetOrderById(id);

            if (order == null)
                return BadRequest();

            if (order.Status == OrderStatus.Accepted || order.Status == OrderStatus.Cancelled)
                return Forbid();

            order.Status = isPaid ? OrderStatus.Accepted : OrderStatus.Cancelled;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return Ok(order);
        }
        private Order? GetOrderById(Guid id)
        {
            return _dbContext.Orders.SingleOrDefault(o => o.Id == id);
        }
        private bool OrderExists(Guid id)
        {
            return _dbContext.Orders.Any(o => o.Id == id);
        }
    }
}
