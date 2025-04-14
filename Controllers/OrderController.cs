using BukovskyCaseStudy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BukovskyCaseStudy.Controllers
{
    [ApiController]
    [Route("api/Orders")]
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
        public List<Models.Order> GetOrderList()
        {
            return new List<Models.Order>();
        }

        [Route("")]
        [HttpPost(Name = "CreateOrder")]
        public void CreateOrder()
        {
        }

        [Route("{id:guid}")]
        [HttpPatch(Name = "ProcessOrder")]
        public void ProcessOrder(bool isPaid)
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
