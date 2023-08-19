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

        modelBuilder.Entity<Order>()
        .HasMany(e => e.Products)
        .WithOne(e => e.Order)
        .HasForeignKey(e => e.OrderId)
        .IsRequired();

        modelBuilder.Entity<Product>()
           .HasKey(b => b.Name);
    }
}
