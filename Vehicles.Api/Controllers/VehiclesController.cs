using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using Vehicles.Core;
using Vehicles.Core.Entities;
using Vehicles.Core.Providers;

namespace Vehicles.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class VehiclesController : ControllerBase
{
    private static readonly ConcurrentDictionary<int, Vehicle> _vehicles = new();
    private readonly ILogger<VehiclesController> _logger;

    private readonly IGuidProvider _guidProvider;
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

    public VehiclesController(IGuidProvider guidProvider, ILogger<VehiclesController> logger)
    {
        _guidProvider = guidProvider;
        _logger = logger;

        _vehicles.TryAdd(1, new Vehicle()
        {
            Id = _guidProvider.New(),
            // GeneralInformation = new GeneralInformation()
            // {
            //     Brand = "BMV",
            //     Model = "BMV",
            //     BodyType = "BMV",
            //     StartOfProduction = "BMV",
            // },
            // Engine = new EngineSpecs()
            // {
            //     Model = "BMV",
            // },
            // Dimensions = new Dimensions()
            // {
            //     Length = 1,
            //     Width = 1,
            //     FrontTrack = 1,
            //     RearTrack = 1,
            //     RideHeight = 1
            // }
        });
    }

    [HttpGet]
    public ICollection<Vehicle> Get()
    {
        return _vehicles.Values.ToArray();
    }
}