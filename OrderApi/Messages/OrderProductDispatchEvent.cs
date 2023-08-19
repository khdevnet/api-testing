using System;

namespace OrderApi.Messages;

public record OrderProductDispatchEvent
{
    public Guid OrderId { get; init; }
    public string Product { get; init; } = string.Empty;
}
