using OrderApi.Core.Domain;
using OrderApi.Core.Repositories;

namespace OrderApi.Infrastructure.Data.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrdersContext _db;

    public OrderRepository(OrdersContext db) => _db = db;

    public async Task AddAsync(Order order) => await _db.AddAsync(order);

    public async Task<Order> GetByIdAsync(Guid orderId) => await _db.Orders.FindAsync(orderId);
}
