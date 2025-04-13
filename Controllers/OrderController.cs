using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BukovskyCaseStudy.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        [Route("[controller]/Orders")]
        [HttpGet(Name = "GetOrderList")] 
        public List<Models.Order> GetOrderList()
        {
            return new List<Models.Order>();
        }

        [Route("[controller]/Orders")]
        [HttpPost(Name = "CreateOrder")]
        public void CreateOrder()
        {
        }

        [Route("[controller]/Orders/{id}")]
        [HttpPatch(Name = "ProcessOrder")]
        public void ProcessOrder(bool isPaid)
        {
        }
    }
}
