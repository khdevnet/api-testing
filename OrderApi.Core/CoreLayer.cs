using Microsoft.Extensions.DependencyInjection;
using OrderApi.Core.Domain.UseCases;

namespace OrderApi.Core;

public static class CoreLayer
{
    public static IServiceCollection RegisterCoreLayer(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<CreateOrderUseCase>();
        serviceCollection.AddSingleton<IOrderIdGenerator, OrderIdGenerator>();

        return serviceCollection;
    }
}
