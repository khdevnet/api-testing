using Microsoft.AspNetCore.Mvc;

namespace Vehicles.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
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
    
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
