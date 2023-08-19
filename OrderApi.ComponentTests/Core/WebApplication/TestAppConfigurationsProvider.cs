using System.Collections.Generic;
using OrderApi.ComponentTests.Core.Fixtures;

namespace OrderApi.ComponentTests.Core.WebApplication;

public class TestAppConfigurationsProvider
{
    private readonly MsSqlDbContainerFixture _dbContainerFixture;

    public TestAppConfigurationsProvider(MsSqlDbContainerFixture dbContainerFixture)
        => _dbContainerFixture = dbContainerFixture;

    public IDictionary<string, string> Get()
        => new Dictionary<string, string>()
        {
            { "ConnectionStrings:VehiclesContext", _dbContainerFixture.DbConnectionString },
        };
}
