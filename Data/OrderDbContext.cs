using BukovskyCaseStudy.Models;
using Microsoft.EntityFrameworkCore;

namespace BukovskyCaseStudy.Data
{

    public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne()
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
    }
}
