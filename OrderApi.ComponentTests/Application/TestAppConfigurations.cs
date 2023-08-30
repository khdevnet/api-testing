using System.Collections.Generic;
using OrderApi.ComponentTests.Application.Infrastructure;
using OrderApi.ComponentTests.Application.Infrastructure.AccountService;

namespace OrderApi.ComponentTests.Application;

public class FeatureTestAppConfigurations : TestAppConfigurations
{
    public FeatureTestAppConfigurations(MsSqlDbContainerMock dbContainerFixtureMock, AccountServiceMock accountServiceMock)
        : base(dbContainerFixtureMock, accountServiceMock)
    {
    }

    public override IDictionary<string, string> Get()
    {
        var configs = base.Get();
        configs.Add("Features:WhatsupProvider", bool.TrueString);

        return configs;
    }
}

public class TestAppConfigurations
{
    private readonly MsSqlDbContainerMock _dbContainerFixtureMock;
    private readonly AccountServiceMock _accountServiceMock;

    public TestAppConfigurations(MsSqlDbContainerMock dbContainerFixtureMock, AccountServiceMock accountServiceMock)
    {
        _dbContainerFixtureMock = dbContainerFixtureMock;
        _accountServiceMock = accountServiceMock;
    }

    public virtual IDictionary<string, string> Get()
        => new Dictionary<string, string>() { { "ConnectionStrings:OrdersContext", _dbContainerFixtureMock.DbConnectionString }, { "Clients:AccountService", _accountServiceMock.GetUrl() } };
}
