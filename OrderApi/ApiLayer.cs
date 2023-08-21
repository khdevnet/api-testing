using Microsoft.Extensions.DependencyInjection;
using OrderApi.Core.Messages;
using OrderApi.IntegrationHandlers;
using OrderApi.Messages;
using SharedKernal;

namespace OrderApi;

public static class ApiLayer
{
    public static IServiceCollection RegisterApiLayer(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IHandleMessages<ApproveOrderCommand>, OrderStatusHandler>();
        serviceCollection.AddScoped<IHandleMessages<RejectOrderCommand>, OrderStatusHandler>();

        return serviceCollection;
    }
}
