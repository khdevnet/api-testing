using System.Threading.Tasks;
using LightBDD.Core.Execution;
using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;

namespace OrderApi.ComponentTests.Application.Infrastructure;

public class MsSqlDbContainerMock : IGlobalResourceSetUp
{
    private const string Password = "yourStrong(!)Password";

    private SqlConnectionStringBuilder _connectionStringBuilder = null!;

    public MsSqlContainer Container;

    public MsSqlDbContainerMock() => Container = CreateMsSqlContainer();

    public string DbConnectionString => _connectionStringBuilder.ConnectionString;

    public async Task SetUpAsync()
    {
        await Container.StartAsync();

        _connectionStringBuilder =
            new SqlConnectionStringBuilder(@$"
                    Server=tcp:localhost,{Container.GetMappedPublicPort(1433)};
                    Initial Catalog=Orders;
                    Persist Security Info=False;
                    User ID=sa;
                    Password={Password};
                    MultipleActiveResultSets=False;
                    Encrypt=False;
                    TrustServerCertificate=True;
                    Connection Timeout=30;");
    }

    public Task TearDownAsync() => Container.DisposeAsync().AsTask();

    private MsSqlContainer CreateMsSqlContainer()
        => new MsSqlBuilder()
            .Build();
}
