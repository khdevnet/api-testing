namespace Vehicles.Core.Entities;

public class EngineSpecs
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }
    public string Power { get; set; }
    public string Model { get; set; }
    public short NumberOfCylinders { get; set; }
    public string FuelInjectionSystem { get; set; }
}

// Engine specs
// Power	306 Hp @ 5000-6250 rpm.
//     Power per litre	153.2 Hp/l
// Torque	450 Nm @ 1800-4500 rpm.
// 331.9 lb.-ft. @ 1800-4500 rpm.
//     Engine layout	Front, Transverse
// Engine Model/Code	B48A20E
//     Engine displacement	1998 cm3
// 121.93 cu. in.
// Number of cylinders	4
// Engine configuration	Inline
//     Cylinder Bore	82 mm
// 3.23 in.
// Piston Stroke	94.6 mm
// 3.72 in.
// Compression ratio	9.5
// Number of valves per cylinder	4
// Fuel injection system	Direct injection
//     Engine aspiration	Twin-power turbo, Intercooler
//     Valvetrain	VALVETRONIC
//     Engine oil capacity	5.25 l
// 5.55 US qt | 4.62 UK qt
// Engine oil specification	Log in to see.
//     Engine systems	Start & Stop System
//     Particulate filter
//