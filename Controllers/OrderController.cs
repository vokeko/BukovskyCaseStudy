using BukovskyCaseStudy.Data;
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
            _logger.LogInformation("Fetching the list of orders.");
            var orders = await _dbContext.Orders.ToListAsync();
            _logger.LogInformation("Successfully fetched {Count} orders.", orders.Count);
            return orders;
        }

        [Route("")]
        [HttpPost(Name = "CreateOrder")]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order)
        {
            _logger.LogInformation("Creating a new order for client: {ClientName}.", order.ClientName);
            order.DateCreated = DateTime.UtcNow;
            _dbContext.Orders.Add(order);

            if (order.OrderItems.Count > 0)
            {
                _logger.LogInformation("Adding {Count} order items to the order.", order.OrderItems.Count);
                _dbContext.OrderItems.AddRange(order.OrderItems.Select(i => { i.OrderId = order.Id; return i; }).ToList()); 
            }

            try
            {
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Order created successfully with ID: {OrderId}.", order.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the order.");
                throw;
            }

            return Ok(order);
        }

        [Route("{id:guid}")]
        [HttpPatch(Name = "ProcessOrder")]
        public async Task<IActionResult> ProcessOrder([FromRoute] Guid id, [FromQuery] bool isPaid)
        {
            _logger.LogInformation("Processing order with ID: {OrderId}.", id);
            var order = GetOrderById(id);

            if (order == null)
            {
                _logger.LogWarning("Order with ID: {OrderId} not found.", id);
                return BadRequest(); 
            }

            if (order.Status == OrderStatus.Accepted || order.Status == OrderStatus.Cancelled)
            {
                _logger.LogWarning("Order with ID: {OrderId} is already in a final state: {Status}.", id, order.Status);
                return Forbid();
            }


            order.Status = isPaid ? OrderStatus.Accepted : OrderStatus.Cancelled;

            try
            {
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Order with ID: {OrderId} processed successfully. New status: {Status}.", id, order.Status);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error while processing order with ID: {OrderId}.", id);
                if (!OrderExists(id))
                {
                    _logger.LogWarning("Order with ID: {OrderId} no longer exists.", id);
                    return NotFound();
                }
                throw;
            }

            return Ok(order);
        }
        private Order? GetOrderById(Guid id)
        {
            _logger.LogDebug("Fetching order with ID: {OrderId}.", id);
            return _dbContext.Orders.SingleOrDefault(o => o.Id == id);
        }
        private bool OrderExists(Guid id)
        {
            return _dbContext.Orders.Any(o => o.Id == id);
        }
    }
}
