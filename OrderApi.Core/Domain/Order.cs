namespace OrderApi.Core.Domain;

public record Order
{
    private readonly List<Product> _products = new();

    public Order(Guid id, Guid accountId)
    {
        Id = id;
        AccountId = accountId;
        Status = OrderStatus.Created;
    }

    protected Order() { }

    public Guid Id { get; init; }

    public Guid AccountId { get; init; }

    public IEnumerable<Product> Products => _products.AsReadOnly();

    public OrderStatus Status { get; init; }

    public Order AddProducts(IEnumerable<string> products)
    {
        products.Select(p => new Product(Id, p, this))
            .ToList().ForEach(_products.Add);

        return this;
    }
}
