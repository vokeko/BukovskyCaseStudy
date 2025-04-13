namespace BukovskyCaseStudy
{
    public class Order
    {
        public Guid OrderNumber { get; set; }
        public string? ClientName { get; set; }
        public DateTime DateCreated { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public Order()
        {
            this.OrderItems = [];
        }
    }
}
