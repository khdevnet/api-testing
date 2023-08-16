using System.Reflection;
using Microsoft.AspNetCore.Mvc.Testing;
using RestEase;
using Testcontainers.MsSql;
using Vehicles.Core;
using Vehicles.ComponentTests.Clients;
using Vehicles.Core.Entities;

namespace Vehicles.ComponentTests;

public class UnitTest1
{
    [Fact]
    public async Task Test2()
    {
        MsSqlContainer? msSqlContainer = new MsSqlBuilder()
            .Build();

        await msSqlContainer.StartAsync();

        var h = msSqlContainer.Hostname;
        var pp = msSqlContainer.GetMappedPublicPort(1433);
        var factory = new WebApplicationFactory<Program>();
        var vehicleClient = RestClient.For<IVehicleClient>(factory.CreateClient());
        IReadOnlyList<Vehicle> d = await vehicleClient.Get();
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