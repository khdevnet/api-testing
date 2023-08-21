using System;
using System.Threading.Tasks;
using OrderApi.Core.Domain;
using OrderApi.Core.Messages;
using OrderApi.Core.Repositories;
using OrderApi.Messages;
using SharedKernal;

namespace OrderApi.IntegrationHandlers;

/// <summary>
/// Order status handler that covers scenarios of approving and rejecting orders
/// </summary>
public class OrderStatusHandler : IHandleMessages<ApproveOrderCommand>, IHandleMessages<RejectOrderCommand>
{
    private readonly IOrderRepository _repository;

    public OrderStatusHandler(IOrderRepository repository) => _repository = repository;

    /// <summary>
    /// Updates order status to Complete and publish OrderProductDispatchEvent for each ordered product
    /// </summary>
    public async Task Handle(ApproveOrderCommand message)
    {
        var order = await GetOrderById(message.OrderId);
        order.Completed();
        await _repository.UpdateAsync(order);
    }

    /// <summary>
    /// Updates order status to rejected
    /// </summary>
    public async Task Handle(RejectOrderCommand message)
    {
        var order = await GetOrderById(message.OrderId);
        order.Rejected();
        await _repository.UpdateAsync(order);
    }

    private async Task<Order> GetOrderById(Guid orderId)
    {
        Order order = await _repository.GetByIdAsync(orderId);

        return order;
    }
}
