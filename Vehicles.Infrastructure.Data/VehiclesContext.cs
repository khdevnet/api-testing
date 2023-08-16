using Microsoft.EntityFrameworkCore;
using Vehicles.Core;
using Vehicles.Core.Entities;

namespace Vehicles.Infrastructure.Data;

public class VehiclesContext : DbContext
{
    public VehiclesContext(DbContextOptions<VehiclesContext> options)
        : base(options)
    {
    }

    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<EngineSpecs> EngineSpecs { get; set; }
    public DbSet<GeneralInformation> GeneralInformations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vehicle>()
            .HasKey(b => b.Id);

        modelBuilder.Entity<EngineSpecs>()
            .HasKey(b => b.Id);

        modelBuilder.Entity<GeneralInformation>()
            .HasKey(b => b.Id);

        modelBuilder.Entity<Vehicle>()
            .HasOne(e => e.GeneralInformation)
            .WithOne(e => e.Vehicle)
            .HasForeignKey<GeneralInformation>(e => e.VehicleId)
            .IsRequired();
    }
}