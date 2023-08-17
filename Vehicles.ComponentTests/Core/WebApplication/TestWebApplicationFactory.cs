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
using Vehicles.ComponentTests.Core.ComponentDependencies;
using Vehicles.Core.Providers;
using Vehicles.ComponentTests.Core.LightBDD;
using WireMock.Server;
using Xunit.Abstractions;

namespace Vehicles.ComponentTests.Core.WebApplication;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private const string EnvironmentName = "ComponentTests";
    private readonly MsSqlDbContainer _dbContainer;
    private readonly ITestOutputHelper? _testOutputHelper;
    private readonly Dictionary<string, string>? _testAppConfigurations;

    public TestWebApplicationFactory(
        MsSqlDbContainer dbContainer,
        ITestOutputHelper? testOutputHelper = null,
        Dictionary<string, string>? testAppConfigurations = null)
    {
        WireMockServer = WireMockServer.Start();
        _dbContainer = dbContainer;
        _testOutputHelper = testOutputHelper;
        _testAppConfigurations = testAppConfigurations;
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

        SetTestOutputLogger(builder, _testOutputHelper);

        builder.UseEnvironment(EnvironmentName);
        builder.UseContentRoot(Directory.GetCurrentDirectory());
        builder.ConfigureAppConfiguration(app =>
        {
            app.AddInMemoryCollection(
                new Dictionary<string, string>()
                {
                    { "ConnectionStrings:VehiclesContext", _dbContainer.DbConnectionString },
                });

            if (_testAppConfigurations is not null)
            {
                app.AddInMemoryCollection(_testAppConfigurations);
            }
        });
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IDistributedCache>();
            services.AddDistributedMemoryCache();
        });
    }

    private static void SetTestOutputLogger(IWebHostBuilder builder, ITestOutputHelper? testOutputHelper)
    {
        if (testOutputHelper is not null)
        {
            builder.UseSerilog((_, loggerConfiguration) =>
            {
                loggerConfiguration.MinimumLevel.Is(LogEventLevel.Verbose);
                loggerConfiguration.Enrich.FromLogContext();
                loggerConfiguration.WriteTo.TestOutput(testOutputHelper, outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Properties:j}{NewLine}{Exception}{NewLine}");
            });
        }
    }
}