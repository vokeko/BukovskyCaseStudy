using Microsoft.EntityFrameworkCore;

namespace BukovskyCaseStudy.Models
{
    [PrimaryKey("Id")]
    public class OrderItem
    {
        public long Id { get; set; }
        public string ItemName { get; set; } = "";
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Guid OrderId { get; set; }
    }
}