using System;
using System.Net;
using System.Threading.Tasks;
using LightBDD.Core.Execution;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace OrderApi.ComponentTests.Application.Infrastructure;

public class AccountServiceMock : IGlobalResourceSetUp
{
    private WireMockServer _server;

    public void SetupGetAccount(Guid accountId, bool response) =>
        _server.Given(Request.Create().UsingGet().WithPath($"/accounts/{accountId}/validate"))
            .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.OK).WithBodyAsJson(response));

    public string GetUrl() => _server.Url;

    public Task SetUpAsync()
    {
        _server = WireMockServer.Start();
        return Task.CompletedTask;
    }
    public Task TearDownAsync()
    {
        _server?.Dispose();
        return Task.CompletedTask;
    }
}
