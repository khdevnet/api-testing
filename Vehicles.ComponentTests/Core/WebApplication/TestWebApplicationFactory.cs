using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using RestEase;
using Serilog;
using Serilog.Events;
using Vehicles.ComponentTests.Clients;
using Vehicles.Core.Providers;
using Vehicles.ComponentTests.Core.LightBDD;
using WireMock.Server;

namespace Vehicles.ComponentTests.Core.WebApplication;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private const string EnvironmentName = "ComponentTests";
    private readonly TestAppConfigurationsProvider _testAppConfigurationsProvider;

    public TestWebApplicationFactory(
        TestAppConfigurationsProvider testAppConfigurationsProvider)
    {
        WireMockServer = WireMockServer.Start();
        _testAppConfigurationsProvider = testAppConfigurationsProvider;
        VehiclesClient = RestClient.For<IVehiclesClient>(CreateClientWithLogger());
    }

    public WireMockServer WireMockServer { get; init; }

    public Mock<IGuidProvider> GuidProviderMock { get; init; } = new Mock<IGuidProvider>();

    public IVehiclesClient VehiclesClient { get; init; }

    public HttpClient CreateClientWithLogger() => CreateDefaultClient(new StepHttpLoggingHandler(new LightBDDTestLogger<StepHttpLoggingHandler>()));

    protected override void Dispose(bool disposing)
    {
        WireMockServer.Dispose();
        base.Dispose(disposing);
    }

    protected override TestServer CreateServer(IWebHostBuilder builder)
    {
        var server = base.CreateServer(builder);
        return server;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        SetLogger(builder);

        builder.UseEnvironment(EnvironmentName);
        builder.UseContentRoot(Directory.GetCurrentDirectory());
        builder.ConfigureAppConfiguration(app =>
        {
            IDictionary<string, string> appConfigurationOverrides = _testAppConfigurationsProvider.Get();
            if (appConfigurationOverrides.Any())
            {
                app.AddInMemoryCollection(appConfigurationOverrides);
            }
        });
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IDistributedCache>();
            services.AddDistributedMemoryCache();
        });
    }

    private static void SetLogger(IWebHostBuilder builder)
    {
        builder.UseSerilog((_, loggerConfiguration) =>
        {
            loggerConfiguration.MinimumLevel.Is(LogEventLevel.Verbose);
            loggerConfiguration.Enrich.FromLogContext();
            loggerConfiguration.WriteTo.File(Path.Combine(Directory.GetCurrentDirectory(), "VehiclesApiTestLogs.txt"), rollingInterval: RollingInterval.Day, outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Properties:j}{NewLine}{Exception}{NewLine}");
        });
    }
}