namespace OrderApi.Core.Domain;
public record Product
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected Product() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public Product(Guid orderId, string name, Order order)
    {
        OrderId = orderId;
        Name = name;
        Order = order;
    }

    public Guid OrderId { get; init; }
    public string Name { get; init; }
    public Order Order { get; init; }
}
