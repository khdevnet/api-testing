namespace OrderApi.Core.Messages;

public record ApproveOrderCommand
{
    public Guid OrderId { get; init; }
}
