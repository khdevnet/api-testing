using System;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace OrderApi.ComponentTests.Core.WebApplication;

public static class WebApplicationFactoryExtensions
{
    public static TService RetrieveService<TStartup, TService>(this WebApplicationFactory<TStartup> factory)
        where TStartup : class
        where TService : class
        => factory.Services.GetService<TService>();

    public static TMessageHandler RetrieveScopedService<TStartup, TMessageHandler>(this WebApplicationFactory<TStartup> factory)
        where TStartup : class
        where TMessageHandler : class
    {
        var serviceScopeFactory = factory.RetrieveService<TStartup, IServiceScopeFactory>();

        if (serviceScopeFactory is null)
            throw new Exception($"Failed to retrieve {nameof(IServiceScopeFactory)} from provided WebApplicationFactory.");

        using var serviceScope = serviceScopeFactory.CreateScope();
        return serviceScope.ServiceProvider.GetRequiredService<TMessageHandler>();
    }
}
