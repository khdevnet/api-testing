using System;
using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

#pragma warning disable CS1998

namespace OrderApi.ComponentTests.Application.Infrastructure.AccountService;

public class AccountServiceMock : IDisposable
{
    private readonly WireMockServer _server;

    public AccountServiceMock()
        => _server = WireMockServer.Start();

    public void SetupAccountValidateRequest(Guid accountId, bool response) =>
        _server.Given(Request.Create().UsingGet().WithPath($"/accounts/{accountId}/validate"))
            .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.OK).WithBodyAsJson(response));

    public void SetupGetAccountRequest(UserAccount account) =>
        _server.Given(Request.Create().UsingGet().WithPath($"/accounts/{account.AccountId}"))
            .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.OK).WithBodyAsJson(new { approved = account.Approved, phoneNumber = account.PhoneNumber }));

    public string GetUrl() => _server.Url;

    public void Dispose() => _server?.Dispose();
}
