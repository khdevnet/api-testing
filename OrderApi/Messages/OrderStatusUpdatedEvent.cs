using System;
using OrderApi.Core.Domain;

namespace OrderApi.Messages;

public record OrderStatusUpdatedEvent
{
    public Guid OrderId { get; init; }
    public OrderStatus Status { get; init; }
}
