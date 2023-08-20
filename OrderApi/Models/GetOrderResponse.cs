using System;

namespace OrderApi.Models;

public record GetOrderResponse
{
    public Guid AccountId { get; init; }

    public Guid OrderId { get; init; }

    public string[] Products { get; init; } = Array.Empty<string>();
}
