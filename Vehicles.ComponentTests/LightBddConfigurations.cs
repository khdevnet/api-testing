using LightBDD.Core.Configuration;
using LightBDD.Core.Dependencies;
using LightBDD.Framework.Configuration;
using LightBDD.Framework.Reporting.Formatters;
using LightBDD.XUnit2;
using Microsoft.AspNetCore.TestHost;
using Vehicles.ComponentTests;
using Vehicles.ComponentTests.Core.Fixtures;
using Vehicles.ComponentTests.Core.WebApplication;

/*
 * This is a way to enable LightBDD - XUnit integration.
 * It is required to do it in all assemblies with LightBDD scenarios.
 * It is possible to either use [assembly:LightBddScope] directly to use LightBDD with default configuration,
 * or customize it in a way that is shown below.
 */
[assembly: LightBddConfigurations]
/*
 * This is a LightBDD specific, experimental attribute enabling inter-class test parallelization
 * (as long as class does not implement IClassFixture<T> nor ICollectionFixture<T> attribute
 * nor have defined named collection [Collection] attribute)
 */
[assembly: ClassCollectionBehavior(AllowTestParallelization = true)]

namespace Vehicles.ComponentTests;

/// <summary>
/// This class extends LightBddScopeAttribute and allows to customize the default configuration of LightBDD.
/// It is also possible here to override OnSetUp() and OnTearDown() methods to execute code that has to be run once, before or after all tests.
/// </summary>
internal class LightBddConfigurations : LightBddScopeAttribute
{
    /// <summary>
    /// This method allows to customize LightBDD behavior.
    /// The code below configures LightBDD to produce also a plain text report after all tests are done.
    /// More information on what can be customized can be found on wiki: https://github.com/LightBDD/LightBDD/wiki/LightBDD-Configuration#configurable-lightbdd-features
    /// </summary>
    protected override void OnConfigure(LightBddConfiguration configuration)
    {
        configuration
            .DependencyContainerConfiguration()
            .UseDefault(ConfigureDI);
        
        configuration.ExecutionExtensionsConfiguration()
            .RegisterGlobalSetUp<MsSqlDbContainerFixture>();

        configuration.ReportWritersConfiguration()
            .AddFileWriter<XmlReportFormatter>("~\\Reports\\FeaturesReport.xml")
            .AddFileWriter<PlainTextReportFormatter>("~\\Reports\\{TestDateTimeUtc:yyyy-MM-dd-HH_mm_ss}_FeaturesReport.txt");
    }

    private void ConfigureDI(IDefaultContainerConfigurator cfg)
    {
        cfg.RegisterType<TestWebApplicationFactory>(InstanceScope.Single);
        cfg.RegisterType<TestAppConfigurationsProvider>(InstanceScope.Single);
        cfg.RegisterType<MsSqlDbContainerFixture>(InstanceScope.Single);
    }
}