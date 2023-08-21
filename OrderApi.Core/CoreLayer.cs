using Microsoft.Extensions.DependencyInjection;
using OrderApi.Core.Services;

namespace OrderApi.Core;

public static class CoreLayer
{
    public static IServiceCollection RegisterCoreLayer(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<CreateOrderService>();
        serviceCollection.AddSingleton<IOrderIdGenerator, OrderIdGenerator>();

        return serviceCollection;
    }
}
