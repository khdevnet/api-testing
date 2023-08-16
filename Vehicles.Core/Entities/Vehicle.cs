namespace Vehicles.Core.Entities;

public class Vehicle
{
    public Guid Id { get; set; }
    public GeneralInformation GeneralInformation { get; set; }
    public EngineSpecs Engine { get; set; }
}

// https://www.auto-data.net/en/bmw-1-series-hatchback-f40-m135i-306hp-xdrive-steptronic-42485
// General information
// Brand	BMW
// Model	1 Series
//     Generation	1 Series Hatchback (F40)
// Modification (Engine)	M135i (306 Hp) xDrive Steptronic
// Start of production	2020 year
//     Powertrain Architecture	Internal Combustion engine
// Body type	Hatchback
//     Seats	5
// Doors	5
// Performance specs
// Fuel consumption (economy) - urban (NEDC, WLTP equivalent)	7.5-8.2 l/100 km
// 31.36 - 28.68 US mpg
// 37.66 - 34.45 UK mpg
// 13.33 - 12.2 km/l
// Fuel consumption (economy) - extra urban (NEDC, WLTP equivalent)	5.6-5.9 l/100 km
// 42 - 39.87 US mpg
// 50.44 - 47.88 UK mpg
// 17.86 - 16.95 km/l
// Fuel consumption (economy) - combined (NEDC, WLTP equivalent)	6.3-6.7 l/100 km
// 37.34 - 35.11 US mpg
// 44.84 - 42.16 UK mpg
// 15.87 - 14.93 km/l
// CO2 emissions (NEDC, WLTP equivalent)	145-155 g/km
// Fuel Type	Petrol (Gasoline)
// Acceleration 0 - 100 km/h	4.8 sec
//     Acceleration 0 - 62 mph	4.8 sec
//     Acceleration 0 - 60 mph (Calculated by Auto-Data.net)	4.6 sec
//     Maximum speed	250 km/h
// 155.34 mph
//     Emission standard	Euro 6d
// Weight-to-power ratio	5 kg/Hp, 200.7 Hp/tonne
// Weight-to-torque ratio	3.4 kg/Nm, 295.1 Nm/tonne

// 2020 BMW 1 Series Hatchback (F40) M135i (306 Hp) xDrive Steptronic | Technical specs, data, fuel consumption, Dimensions: https://www.auto-data.net/en/bmw-1-series-hatchback-f40-m135i-306hp-xdrive-steptronic-42485