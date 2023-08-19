using System.Collections.Generic;
using OrderApi.ComponentTests.Application.Infrastructure;

namespace OrderApi.ComponentTests.Application;

public class TestAppConfigurationsProvider
{
    private readonly MsSqlDbContainerMock _dbContainerFixture;
    private readonly AccountServiceMock _accountServiceMock;

    public TestAppConfigurationsProvider(MsSqlDbContainerMock dbContainerFixture, AccountServiceMock accountServiceMock)
    {
        _dbContainerFixture = dbContainerFixture;
        _accountServiceMock = accountServiceMock;
    }

    public IDictionary<string, string> Get()
        => new Dictionary<string, string>()
        {
            { "ConnectionStrings:VehiclesContext", _dbContainerFixture.DbConnectionString },
            { "Clients:Account", _accountServiceMock.GetUrl() }
        };
}
