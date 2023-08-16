using RestEase;
using Vehicles.Core;
using Vehicles.Core.Entities;

namespace Vehicles.ComponentTests.Clients;

public interface IVehicleClient
{
    [Get("vehicles")]
    Task<IReadOnlyList<Vehicle>> Get();
}