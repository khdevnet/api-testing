using RestEase;
using Vehicles.Core;

namespace Vehicles.ComponentTests.Clients;

public interface IVehicleClient
{
    [Get("vehicles")]
    Task<IReadOnlyList<Vehicle>> Get();
}