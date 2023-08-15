using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using Vehicles.Core;

namespace Vehicles.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class VehiclesController : ControllerBase
{
    // Scenario1
    // Create new vehicle
    // SetEngineSpecs
    // SetGeneralInformation
    // Publish to Catalog
    
    // Scenario2
    // Find BWM vehicle
    // RemoveEngineSpecs
    // SetGeneralInformation
    // Publish to Catalog
    
    // Scenario3
    // Find BWM vehicle
    // Set As Deprecated
    // Notify Moderator
    // Remove from catalog
    
    private static readonly ConcurrentDictionary<int, Vehicle> _vehicles = new ();

    private readonly ILogger<VehiclesController> _logger;

    public VehiclesController(ILogger<VehiclesController> logger)
    {
        _logger = logger;

        _vehicles.TryAdd(1, new Vehicle(new GeneralInformation()
        {
             Brand = "BMV",
             Model =  "BMV",
             BodyType =  "BMV",
             StartOfProduction = "BMV",
        }, new EngineSpecs()
        {
             Model = "BMV",
        }, new Dimensions()
        {
             Length = 1,
             Width = 1,
             FrontTrack = 1,
             RearTrack = 1,
             RideHeight = 1
        }));
    }

    [HttpGet]
    public ICollection<Vehicle> Get()
    {
        return _vehicles.Values.ToArray();
    }
}
