using LightBDD.Core.Configuration;
using LightBDD.Core.Dependencies;
using LightBDD.XUnit2;
using OrderApi.ComponentTests;
using OrderApi.ComponentTests.Core.Fixtures;
using Vehicles.ComponentTests.Core.WebApplication;

[assembly: ClassCollectionBehavior(AllowTestParallelization = true)]
[assembly: ConfiguredLightBddScope]

namespace OrderApi.ComponentTests;

internal class ConfiguredLightBddScopeAttribute : LightBddScopeAttribute
{
    protected override void OnConfigure(LightBddConfiguration configuration)
    {
        configuration.DependencyContainerConfiguration()
            .UseDefault(ConfigureDI);

        configuration.ExecutionExtensionsConfiguration()
            .RegisterGlobalSetUp<MsSqlDbContainerFixture>();
    }

    private void ConfigureDI(IDefaultContainerConfigurator cfg)
    {
        cfg.RegisterType<TestWebApplicationFactory>(InstanceScope.Single);
        cfg.RegisterType<MsSqlDbContainerFixture>(InstanceScope.Single);
    }
}
