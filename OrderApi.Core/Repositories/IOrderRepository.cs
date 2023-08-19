using OrderApi.Core.Domain;

namespace OrderApi.Core.Repositories;

public interface IOrderRepository
{
    Task AddAsync(Order order);

    Task<Order> GetByIdAsync(Guid orderId);
}
