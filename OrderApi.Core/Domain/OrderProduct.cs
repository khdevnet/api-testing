namespace OrderApi.Core.Domain;
#pragma warning disable CS8618

public record OrderProduct
{
    protected OrderProduct()
    {
    }

    public OrderProduct(Guid orderId, string name)
    {
        OrderId = orderId;
        Name = name;
    }

    public Guid OrderId { get; init; }

    public string Name { get; init; }

    public Order Order { get; init; }
}
