using System;
using Microsoft.Extensions.DependencyInjection;

namespace OrderApi.ComponentTests.Application;

public static class WebApplicationFactoryExtensions
{
    public static TService RetrieveService<TService>(this TestWebApplicationFactory factory)
        where TService : class
        => factory.Services.GetService<TService>();

    public static TMessageHandler RetrieveScopedService<TMessageHandler>(this TestWebApplicationFactory factory)
        where TMessageHandler : class
    {
        var serviceScopeFactory = factory.RetrieveService<IServiceScopeFactory>();

        if (serviceScopeFactory is null)
            throw new Exception($"Failed to retrieve {nameof(IServiceScopeFactory)} from provided WebApplicationFactory.");

        using var serviceScope = serviceScopeFactory.CreateScope();
        return serviceScope.ServiceProvider.GetRequiredService<TMessageHandler>();
    }
}
