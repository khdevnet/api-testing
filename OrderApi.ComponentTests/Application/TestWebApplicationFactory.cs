using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OrderApi.ComponentTests.Application.Clients;
using OrderApi.ComponentTests.Application.Infrastructure;
using OrderApi.ComponentTests.LightBDD;
using RestEase;
using Serilog;
using Serilog.Events;
using SharedKernal;
using System.IO;
using System.Linq;
using System.Net.Http;
using WireMock.Server;

namespace OrderApi.ComponentTests.Application;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private const string EnvironmentName = "ComponentTests";
    private readonly TestAppConfigurationsProvider _testAppConfigurationsProvider;

    public IOrdersClient OrdersClient { get; private set; }
    public AccountServiceMock AccountClientMock { get; private set; }

    public TestWebApplicationFactory(
        TestAppConfigurationsProvider testAppConfigurationsProvider,
        AccountServiceMock accountClientMock)
    {
        WireMockServer = WireMockServer.Start();
        _testAppConfigurationsProvider = testAppConfigurationsProvider;
        AccountClientMock = accountClientMock;
        OrdersClient = RestClient.For<IOrdersClient>(CreateClientWithLogger());
    }

    public WireMockServer WireMockServer { get; init; }

    // public Mock<IGuidProvider> GuidProviderMock { get; init; } = new Mock<IGuidProvider>();


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
            var appConfigurationOverrides = _testAppConfigurationsProvider.Get();
            if (appConfigurationOverrides.Any())
            {
                app.AddInMemoryCollection(appConfigurationOverrides);
            }
        });
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IBus>();
            services.AddScoped<IBus, MessageBusMock>();
        });
    }

    private static void SetLogger(IWebHostBuilder builder) =>
        builder.UseSerilog((_, loggerConfiguration) =>
        {
            loggerConfiguration.MinimumLevel.Is(LogEventLevel.Verbose);
            loggerConfiguration.Enrich.FromLogContext();
            loggerConfiguration.WriteTo.File(Path.Combine(Directory.GetCurrentDirectory(), "VehiclesApiTestLogs.txt"), rollingInterval: RollingInterval.Day, outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Properties:j}{NewLine}{Exception}{NewLine}");
        });
}
