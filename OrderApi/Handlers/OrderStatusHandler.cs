using System;
using System.Threading.Tasks;
using OrderApi.Core.Domain;
using OrderApi.Core.Repositories;
using OrderApi.Messages;
using Rebus.Bus;
using Rebus.Handlers;

namespace OrderApi.Handlers;

/// <summary>
/// Order status handler that covers scenarios of approving and rejecting orders
/// </summary>
public class OrderStatusHandler : IHandleMessages<ApproveOrderCommand>, IHandleMessages<RejectOrderCommand>
{
    private readonly IOrderRepository _repository;
    private readonly IBus _bus;

    public OrderStatusHandler(IOrderRepository repository, IBus bus)
    {
        _repository = repository;
        _bus = bus;
    }

    /// <summary>
    /// Updates order status to Complete and publish OrderProductDispatchEvent for each ordered product
    /// </summary>
    public async Task Handle(ApproveOrderCommand message)
    {
        var order = await UpdateOrderStatus(message.OrderId, OrderStatus.Complete);
        if (order == null)
        {
            return;
        }

        foreach (var product in order.Products)
        {
            await _bus.Publish(new OrderProductDispatchEvent { OrderId = order.Id, Product = product.Name });
        }
    }


    /// <summary>
    /// Updates order status to rejected
    /// </summary>
    public async Task Handle(RejectOrderCommand message)
        => await UpdateOrderStatus(message.OrderId, OrderStatus.Rejected);

    private async Task<Order?> UpdateOrderStatus(Guid orderId, OrderStatus status)
    {
        var order = await _repository.GetByIdAsync(orderId);
        if (order is not { Status: OrderStatus.Created })
        {
            return null;
        }

        order = order with { Status = status };
        await _repository.AddAsync(order);

        await _bus.Publish(new OrderStatusUpdatedEvent { OrderId = order.Id, Status = order.Status });
        return order;
    }
}
