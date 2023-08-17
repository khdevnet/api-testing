using Features.Common;
using Vehicles.ComponentTests.Core.WebApplication;

namespace Vehicles.ComponentTests.Features;

internal class Vehicles_steps : Base_api_steps
{
    public TestWebApplicationFactory App { get; }

    public Vehicles_steps(TestWebApplicationFactory app)
    {
        App = app;
    }


    public async Task When_get_vehicles()
    {
        Response = await App.VehiclesClient.Get();
    }
}