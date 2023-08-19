using System;
using System.IO;
using LightBDD.Core.Configuration;
using LightBDD.Core.Dependencies;
using LightBDD.XUnit2;
using OrderApi.ServiceTests;
using OrderApi.ServiceTests.Infrastructure;

[assembly: ClassCollectionBehavior(AllowTestParallelization = true)]
[assembly: ConfiguredLightBddScope]

namespace OrderApi.ServiceTests
{
    internal class ConfiguredLightBddScopeAttribute : LightBddScopeAttribute
    {
        protected override void OnConfigure(LightBddConfiguration configuration)
        {
            configuration.DependencyContainerConfiguration()
                .UseDefault(ConfigureDI);

            configuration.ExecutionExtensionsConfiguration()
                //ensures the db is deleted as last one (reverse tear down order)
                .RegisterGlobalTearDown("db cleanup", () => File.Delete(Path.Combine(AppContext.BaseDirectory, ".orders.db")))
                //ensures the TestServer is initialized before TestBus and disposed after
                .RegisterGlobalSetUp<TestServer>()
                .RegisterGlobalSetUp<TestBus>();
        }

        private void ConfigureDI(IDefaultContainerConfigurator cfg)
        {
            cfg.RegisterType<TestServer>(InstanceScope.Single);
            cfg.RegisterType<TestBus>(InstanceScope.Single);
            cfg.RegisterType<MockAccountService>(InstanceScope.Single);
        }
    }
}
