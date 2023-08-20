using System.Collections.Generic;
using OrderApi.ComponentTests.Application.Infrastructure;

namespace OrderApi.ComponentTests.Application;

public class TestAppConfigurations
{
    private readonly MsSqlDbContainerMock _dbContainerFixtureMock;
    private readonly AccountServiceMock _accountServiceMock;

    public TestAppConfigurations(MsSqlDbContainerMock dbContainerFixtureMock, AccountServiceMock accountServiceMock)
    {
        _dbContainerFixtureMock = dbContainerFixtureMock;
        _accountServiceMock = accountServiceMock;
    }

    public IDictionary<string, string> Get()
        => new Dictionary<string, string>()
        {
            { "ConnectionStrings:OrdersContext", _dbContainerFixtureMock.DbConnectionString },
            { "Clients:AccountService", _accountServiceMock.GetUrl() }
        };
}
