using System.Diagnostics;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;

namespace Vehicles.ComponentTests.Core.ComponentDependencies;

[CollectionDefinition(nameof(MsSqlDbContainerFixture))]
public class MsSqlDbContainerFixture : ICollectionFixture<MsSqlDbContainer>
{
}

public class MsSqlDbContainer : IAsyncLifetime
{
    private const string Password = "yourStrong(!)Password";

    private readonly IOutputConsumer _consumer = Consume.RedirectStdoutAndStderrToStream(new MemoryStream(), new MemoryStream());

    private SqlConnectionStringBuilder _connectionStringBuilder = null!;

    public MsSqlContainer Container;

    public MsSqlDbContainer() => Container = CreateMsSqlContainer();

    public string DbConnectionString => _connectionStringBuilder.ConnectionString;

    public async Task InitializeAsync()
    {
        await Container.StartAsync();
        _connectionStringBuilder =
            new SqlConnectionStringBuilder(@$"
                    Server=tcp:localhost,{Container.GetMappedPublicPort(1433)};
                    Initial Catalog=Vehicles;
                    Persist Security Info=False;
                    User ID=sa;
                    Password={Password};
                    MultipleActiveResultSets=False;
                    Encrypt=False;
                    TrustServerCertificate=True;
                    Connection Timeout=30;");

        CreateDatabase(_connectionStringBuilder.InitialCatalog, _connectionStringBuilder.ConnectionString);
    }

    public Task DisposeAsync() => Container.DisposeAsync().AsTask();

    private MsSqlContainer CreateMsSqlContainer()
        => new MsSqlBuilder()
            .Build();

    private void CreateDatabase(string databaseName, string connectionString)
    {
        var masterBuilder = new SqlConnectionStringBuilder(connectionString)
        {
            InitialCatalog = "master",
        };

        using var myConn = new SqlConnection(masterBuilder.ConnectionString);
        using var myCommand = new SqlCommand($"CREATE DATABASE [{databaseName}]", myConn);

        try
        {
            myConn.Open();
            myCommand.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            throw;
        }
    }
}