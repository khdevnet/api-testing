using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Core.ExternalServices;
using OrderApi.Core.ExternalServices.AccountService;
using OrderApi.Infrastructure.ExternalServices.SmsService;
using RestEase.HttpClientFactory;
using SharedKernal;

namespace OrderApi.Infrastructure.ExternalServices;

public static class ExternalServicesLayer
{
    public static IServiceCollection RegisterExternalServicesLayer(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddSingleton<IBus, AzureServiceBus>();
        serviceCollection.AddHttpClient<IAccountServiceClient, AccountServiceClient>(cfg => cfg.BaseAddress = new Uri(configuration["Clients:AccountService"]!));
        
        serviceCollection.AddRestEaseClient<ISmsProvider>(configuration["Clients:SmsService"]);
        return serviceCollection;
    }
}
