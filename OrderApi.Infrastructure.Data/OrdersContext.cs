using Microsoft.EntityFrameworkCore;
using OrderApi.Core.Domain;

namespace OrderApi.Infrastructure.Data;

public class OrdersContext : DbContext
{
    public OrdersContext(DbContextOptions<OrdersContext> options)
        : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .HasKey(b => b.Id);
    }
}
