using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BukovskyCaseStudy.Models
{

    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
    }
}
