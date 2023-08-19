using System;

namespace OrderApi.Messages;

public record ApproveOrderCommand
{
    public Guid OrderId { get; init; }
}
