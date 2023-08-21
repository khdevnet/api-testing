#pragma warning disable CS8618
namespace OrderApi.Core.Domain;

public record Order
{
    private List<OrderProduct> _products;

    public Order(Guid id, Guid accountId)
    {
        Id = id;
        AccountId = accountId;
        Status = OrderStatus.Created;
        _products = new List<OrderProduct>();
    }

    protected Order()
    {
    }

    public Guid Id { get; private set; }

    public Guid AccountId { get; private set; }

    public OrderStatus Status { get; private set; }

    public IEnumerable<OrderProduct> Products
    {
        get => _products.AsReadOnly();
        private set => _products = value.ToList();
    }

    public Order Completed()
    {
        Status = OrderStatus.Complete;

        return this;
    }

    public Order Rejected()
    {
        Status = OrderStatus.Rejected;

        return this;
    }

    public Order AddProducts(IEnumerable<string> products)
    {
        products.Select(p => new OrderProduct(Id, p))
            .ToList().ForEach(_products.Add);

        return this;
    }
}
