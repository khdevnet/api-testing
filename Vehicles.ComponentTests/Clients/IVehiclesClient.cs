using RestEase;

namespace Vehicles.ComponentTests.Clients;

public interface IVehiclesClient
{
    [Get("vehicles")]
    [AllowAnyStatusCode]
    public Task<HttpResponseMessage> Get();
}