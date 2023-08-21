using System;
using OrderApi.Core.Domain;

namespace OrderApi.Models;

public record GetOrderResponse
{
    public Guid AccountId { get; init; }

    public OrderStatus Status { get; init; }

    public Guid OrderId { get; init; }

    public string[] Products { get; init; } = Array.Empty<string>();
}
