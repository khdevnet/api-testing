namespace Vehicles.Core.Entities;

public class GeneralInformation
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string StartOfProduction { get; set; }
    public string BodyType { get; set; }
}