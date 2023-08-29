using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OrderApi.Infrastructure.Data;

public class ApplyMigrationsPostStartup : IHostedService
{
    private readonly IServiceProvider _provider;
    private static readonly SemaphoreSlim _lock = new(1, 1);
    public ApplyMigrationsPostStartup(IServiceProvider provider) => _provider = provider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Migrate latest database changes during startup
        using var scope = _provider.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<OrdersContext>();

        // Here is the migration executed
        await _lock.WaitAsync(cancellationToken);
        await dbContext.Database.MigrateAsync(cancellationToken);
        _lock.Release();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
