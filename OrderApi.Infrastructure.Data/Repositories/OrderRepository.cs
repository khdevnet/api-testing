using Microsoft.Extensions.Logging;
using OrderApi.Core.Domain;
using OrderApi.Core.Repositories;

namespace OrderApi.Infrastructure.Data.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrdersContext _db;
    private readonly ILogger<OrderRepository> _logger;

    public OrderRepository(OrdersContext db, ILogger<OrderRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task AddAsync(Order order)
    {
        try
        {
            _db.Add(order);
            await _db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error");

            throw;
        }
    }

    public async Task UpdateAsync(Order order)
    {
        try
        {
            _db.Update(order);
            await _db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error");

            throw;
        }
    }

    public async Task<Order> GetByIdAsync(Guid orderId) => await _db.Orders.FindAsync(orderId);
}
