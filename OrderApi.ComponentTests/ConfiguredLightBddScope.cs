using LightBDD.Core.Configuration;
using LightBDD.Core.Dependencies;
using LightBDD.XUnit2;
using OrderApi.ComponentTests;
using OrderApi.ComponentTests.Application;
using OrderApi.ComponentTests.Application.Infrastructure;

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
            .RegisterGlobalSetUp<MsSqlDbContainerMock>()
            .RegisterGlobalSetUp<AccountServiceMock>();
    }

    private void ConfigureDI(IDefaultContainerConfigurator cfg)
    {
        cfg.RegisterType<TestWebApplicationFactory>(InstanceScope.Single);
        cfg.RegisterType<TestAppConfigurations>(InstanceScope.Single);
        cfg.RegisterType<MsSqlDbContainerMock>(InstanceScope.Single);
        cfg.RegisterType<AccountServiceMock>(InstanceScope.Single);
    }
}
