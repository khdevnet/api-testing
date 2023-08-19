using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Core.ExternalServices;
using OrderApi.Infrastructure.ExternalServices;
using SharedKernal;

namespace OrderApi.Core;

public static class ExternalServicesLayer
{
    public static IServiceCollection RegisterExternalServicesLayer(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<IBus, AzureServiceBus>();
        serviceCollection.AddHttpClient<IAccountServiceClient, AccountServiceClient>(cfg => cfg.BaseAddress = new Uri(configuration["Clients:AccountService"]!));
        return serviceCollection;
    }
}
