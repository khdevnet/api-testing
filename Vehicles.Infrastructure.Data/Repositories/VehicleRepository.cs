using Microsoft.EntityFrameworkCore;
using Vehicles.Core;
using Vehicles.Core.Entities;
using Vehicles.Core.Repositories;

namespace Vehicles.Infrastructure.Data.Repositories;

internal class VehicleRepository : IVehicleRepository
{
    private readonly VehiclesContext _context;

    public VehicleRepository(VehiclesContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Vehicle>> Get()
    {
        return await _context.Vehicles.ToListAsync();
    }
}