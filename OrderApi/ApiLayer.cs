using Microsoft.Extensions.DependencyInjection;
using OrderApi.IntegrationHandlers;

namespace OrderApi.Core;

public static class ApiLayer
{
    public static IServiceCollection RegisterApiLayer(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<OrderStatusHandler>();
        return serviceCollection;
    }
}
