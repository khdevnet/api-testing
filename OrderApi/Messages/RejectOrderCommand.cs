using System;

namespace OrderApi.Messages;

public record RejectOrderCommand
{
    public Guid OrderId { get; init; }
}
