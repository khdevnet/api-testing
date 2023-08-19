using System;

namespace OrderApi.Messages;

public record OrderCreatedEvent
{
    public Guid OrderId { get; init; }
}
