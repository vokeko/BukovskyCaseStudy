using Microsoft.EntityFrameworkCore;

namespace BukovskyCaseStudy.Models
{
    [PrimaryKey ("Id")]
    public class Order
    {
        public Guid Id { get; set; }
        public string? ClientName { get; set; }
        public DateTime DateCreated { get; set; }
        public List<OrderItem> OrderItems { get; set; } = [];
        public OrderStatus Status { get; set; } = OrderStatus.New;
    }

    public enum OrderStatus
    {
        New,
        Accepted,
        Cancelled,
    }
}
