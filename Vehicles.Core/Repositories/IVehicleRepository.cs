using Vehicles.Core.Entities;

namespace Vehicles.Core.Repositories;

public interface IVehicleRepository
{
    Task<IReadOnlyList<Vehicle>> Get();
}