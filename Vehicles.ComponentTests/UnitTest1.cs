using System.Reflection;
using Microsoft.AspNetCore.Mvc.Testing;
using RestEase;
using Testcontainers.MsSql;
using Vehicles.Core;
using Vehicles.ComponentTests.Clients;

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
}