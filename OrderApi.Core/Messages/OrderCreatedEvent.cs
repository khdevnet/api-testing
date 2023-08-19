namespace OrderApi.Core.Messages;

public record OrderCreatedEvent
{
    public Guid OrderId { get; init; }
}
