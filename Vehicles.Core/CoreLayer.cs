using Microsoft.Extensions.DependencyInjection;
using Vehicles.Core.Providers;

namespace Vehicles.Core;

public static class CoreLayer
{
    public static IServiceCollection RegisterCoreLayer(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IGuidProvider, GuidProvider>();
        return serviceCollection;
    }
}