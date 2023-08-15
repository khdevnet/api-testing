using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using Vehicles.Core;

namespace Vehicles.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EngineSpecsController : ControllerBase
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
    
    private static readonly ConcurrentDictionary<int, EngineSpecs> _engineSpecs = new ();

    private readonly ILogger<VehiclesController> _logger;

    public EngineSpecsController(ILogger<VehiclesController> logger)
    {
        _logger = logger;

        _engineSpecs.TryAdd(1,  new EngineSpecs());
    }

    [HttpGet]
    public IEnumerable<EngineSpecs> Get()
    {
        return _engineSpecs.Values.ToArray();
    }
}
