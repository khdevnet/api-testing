using OrderApi.Core.Domain;

namespace OrderApi.Core.Messages;

public record OrderStatusUpdatedEvent
{
    public Guid OrderId { get; init; }

    public OrderStatus Status { get; init; }
}
