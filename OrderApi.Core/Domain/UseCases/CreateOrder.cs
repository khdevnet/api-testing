namespace OrderApi.Core.Domain.UseCases;

public record CreateOrder
{
    public CreateOrder(Guid accountId, string[] products)
    {
        AccountId = accountId;
        Products = products;
    }

    public Guid AccountId { get; init; }

    public string[] Products { get; init; } = Array.Empty<string>();
}
