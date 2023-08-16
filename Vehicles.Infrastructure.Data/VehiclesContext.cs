using Microsoft.EntityFrameworkCore;
using Vehicles.Core;

namespace Vehicles.Infrastructure.Data;

public class VehiclesContext : DbContext
{
    public DbSet<Vehicle> Vehicles { get; set; }
}