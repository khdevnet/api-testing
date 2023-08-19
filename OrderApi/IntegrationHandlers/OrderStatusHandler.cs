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
        => await UpdateOrderStatus(message.OrderId, OrderStatus.Complete);


    /// <summary>
    /// Updates order status to rejected
    /// </summary>
    public async Task Handle(RejectOrderCommand message)
        => await UpdateOrderStatus(message.OrderId, OrderStatus.Rejected);

    private async Task UpdateOrderStatus(Guid orderId, OrderStatus status)
    {
        var order = await _repository.GetByIdAsync(orderId);
        if (order is not { Status: OrderStatus.Created })
        {
            throw new ApplicationException("Order not exist.");
        }

        order = order with { Status = status };
        await _repository.AddAsync(order);
    }
}
