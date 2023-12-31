﻿using System.Diagnostics;
using System;
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
                    Initial Catalog=OrdersContext;
                    Persist Security Info=False;
                    User ID=sa;
                    Password={Password};
                    MultipleActiveResultSets=False;
                    Encrypt=False;
                    TrustServerCertificate=True;
                    Connection Timeout=30;");

        CreateDatabase(_connectionStringBuilder.InitialCatalog, _connectionStringBuilder.ConnectionString);
    }

    public Task TearDownAsync() => Container.DisposeAsync().AsTask();

    private MsSqlContainer CreateMsSqlContainer()
        => new MsSqlBuilder()
        .WithPortBinding(56789)
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
