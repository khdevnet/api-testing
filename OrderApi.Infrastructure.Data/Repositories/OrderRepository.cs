using OrderApi.Core.Domain;
using OrderApi.Core.Repositories;

namespace OrderApi.Infrastructure.Data.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrdersContext _db;

    public OrderRepository(OrdersContext db)
        => _db = db;

    public async Task AddAsync(Order order)
    {
        _db.Add(order);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Order order)
    {
        _db.Update(order);
        await _db.SaveChangesAsync();
    }

    public async Task<Order> GetByIdAsync(Guid orderId) => await _db.Orders.FindAsync(orderId);
}
