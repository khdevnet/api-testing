using System;

namespace OrderApi.ComponentTests.Application.Infrastructure.AccountService;

public static class TestAccounts
{
    public static readonly UserAccount JohnDoe = new();
}

public record UserAccount
{
    public Guid AccountId { get; init; } = new Guid("DD3B5A02-F730-4762-8BBD-6AB86418B04D");

    public bool Approved { get; init; } = true;

    public string PhoneNumber { get; init; } = "+31636363634";
}
