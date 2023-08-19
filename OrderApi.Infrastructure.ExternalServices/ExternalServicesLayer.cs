﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Core.ExternalServices;
using SharedKernal;

namespace OrderApi.Infrastructure.ExternalServices;

public static class ExternalServicesLayer
{
    public static IServiceCollection RegisterExternalServicesLayer(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<IBus, AzureServiceBus>();
        serviceCollection.AddHttpClient<IAccountServiceClient, AccountServiceClient>(cfg => cfg.BaseAddress = new Uri(configuration["Clients:AccountService"]!));
        return serviceCollection;
    }
}
