using BukovskyCaseStudy.Data;
using BukovskyCaseStudy.Models;
using Microsoft.EntityFrameworkCore;

namespace BukovskyCaseStudy.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrderListAsync();
        Task<Order> CreateOrderAsync(Order order);
        Task<Order?> ProcessOrderAsync(Guid id, bool isPaid);
    }

    public class OrderService(OrderDbContext dbContext, ILogger<OrderService> logger) : IOrderService
    {
        private readonly OrderDbContext _dbContext = dbContext;
        private readonly ILogger<OrderService> _logger = logger;

        public async Task<IEnumerable<Order>> GetOrderListAsync()
        {
            _logger.LogInformation("Fetching the list of orders.");
            return await _dbContext.Orders.ToListAsync();
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            _logger.LogInformation("Creating a new order for client: {ClientName}.", order.ClientName);
            order.DateCreated = DateTime.UtcNow;
            _dbContext.Orders.Add(order);

            if (order.OrderItems.Count > 0)
            {
                _logger.LogInformation("Adding {Count} order items to the order.", order.OrderItems.Count);
                _dbContext.OrderItems.AddRange(order.OrderItems.Select(i => { i.OrderId = order.Id; return i; }).ToList());
            }

            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Order created successfully with ID: {OrderId}.", order.Id);
            return order;
        }

        public async Task<Order?> ProcessOrderAsync(Guid id, bool isPaid)
        {
            _logger.LogInformation("Processing order with ID: {OrderId}.", id);
            var order = await _dbContext.Orders.SingleOrDefaultAsync(o => o.Id == id);

            if (order == null) return null;

            if (order.Status == OrderStatus.Accepted || order.Status == OrderStatus.Cancelled)
            {
                _logger.LogWarning("Order with ID: {OrderId} is already in a final state: {Status}.", id, order.Status);
                throw new InvalidOperationException("Order is in a final state.");
            }

            order.Status = isPaid ? OrderStatus.Accepted : OrderStatus.Cancelled;
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Order with ID: {OrderId} processed successfully. New status: {Status}.", id, order.Status);
            return order;
        }
    }
}
