using System.Linq.Expressions;
using LightBDD.Core.Dependencies;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;
using RestEase;
using Vehicles.ComponentTests.Clients;
using Vehicles.ComponentTests.Core.ComponentDependencies;
using Vehicles.ComponentTests.Core.WebApplication;
using Xunit.Abstractions;

namespace Vehicles.ComponentTests;

public abstract class BaseComponentTest : FeatureFixture
{
    protected BaseComponentTest(
        MsSqlDbContainer dbContainer,
        ITestOutputHelper? testOutputHelper = null,
        Dictionary<string, string>? testAppConfigurations = null)
    {
        App = new TestWebApplicationFactory(dbContainer, testOutputHelper, testAppConfigurations);
        // Setup global mocks
        // App.DependencyMock
        //     .Setup(u => u.Get(It.IsAny<Specification>()))
        //     .Returns(Task.FromResult(new Entity { Id = Id }));

        Client = App.CreateClientWithLogger();
        VehiclesClient = CreateRestClient<IVehiclesClient>();
        DbContainer = dbContainer;
    }

    public MsSqlDbContainer DbContainer { get; }

    protected HttpClient Client { get; init; }

    protected IVehiclesClient VehiclesClient { get; init; }

    protected TestWebApplicationFactory App { get; init; }

    public void Dispose() => App.Dispose();

    protected TClient CreateRestClient<TClient>()
        => new RestClient(Client).For<TClient>();

    protected abstract object CreateFeatureContext(IDependencyResolver x);

    protected Task RunScenarioAsync<TSteps>(params Expression<Func<TSteps, Task>>[] steps)
        => Runner.WithContext(ctx => (TSteps)CreateFeatureContext(ctx))
            .RunScenarioAsync(steps);
}