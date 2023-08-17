using Features.Common;
using LightBDD.Core.Dependencies;
using LightBDD.XUnit2;
using Vehicles.ComponentTests;
using Vehicles.ComponentTests.Core.ComponentDependencies;
using Vehicles.ComponentTests.Features;
using Xunit.Abstractions;

namespace Features;

[Collection(nameof(MsSqlDbContainerFixture))]
public class Vehicles : BaseComponentTest
{
    private readonly MsSqlDbContainer _msSqlDbContainer;
    private readonly ITestOutputHelper _testOutputHelper;

    public Vehicles(MsSqlDbContainer msSqlDbContainer, ITestOutputHelper testOutputHelper)
        : base(msSqlDbContainer, testOutputHelper)
    {
        _msSqlDbContainer = msSqlDbContainer;
        _testOutputHelper = testOutputHelper;
    }

    [Scenario]
    public async Task Get_vehicles()
    {
        await RunScenarioAsync<Vehicles_steps>(
            _ => _.When_get_vehicles(),
            _ => _.Then_response_is_ok());
    }

    protected override object CreateFeatureContext(IDependencyResolver x)
        => new Vehicles_steps(App);

    //
    // private static void SeedEngineSpecs(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<EngineSpecs>().HasData(
    //         new EngineSpecs
    //         {
    //             Id = new Guid("7BBDDF6C-92C2-411E-8570-1A920968C4BB"),
    //             Model = "B48A20E",
    //             Power = "306 Hp @ 5000-6250 rpm.",
    //             NumberOfCylinders = 4,
    //             FuelInjectionSystem = "Direct injection"
    //         });
    // }
}