using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;
using Vehicles.ComponentTests.Features;

namespace Features;

public class Vehicles : FeatureFixture
{
    [Scenario]
    public async Task Get_vehicles()
    {
        await Runner
            .WithContext<Vehicles_steps>()
            .RunScenarioAsync(
                _ => _.When_get_vehicles(),
                _ => _.Then_response_is_ok());
    }

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