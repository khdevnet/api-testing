using Microsoft.Extensions.DependencyInjection;

namespace OrderApi.Core;

public static class CoreLayer
{
    public static IServiceCollection RegisterCoreLayer(this IServiceCollection serviceCollection)
        => serviceCollection;
}
