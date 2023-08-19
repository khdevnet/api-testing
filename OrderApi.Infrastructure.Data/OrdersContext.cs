using Microsoft.EntityFrameworkCore;
using OrderApi.Core.Domain;
#pragma warning disable CS8618

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

        modelBuilder.Entity<Order>()
            .HasMany(e => e.Products)
            .WithOne(e => e.Order)
            .HasForeignKey(e => e.OrderId)
            .IsRequired();

        modelBuilder.Entity<OrderProduct>()
            .HasKey(nameof(OrderProduct.Name), nameof(OrderProduct.OrderId));
    }
}
