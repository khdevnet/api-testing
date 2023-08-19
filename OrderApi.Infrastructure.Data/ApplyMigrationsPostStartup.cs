using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OrderApi.Infrastructure.Data;

public class ApplyMigrationsPostStartup : IHostedService
{
    private readonly IServiceProvider _provider;

    public ApplyMigrationsPostStartup(IServiceProvider provider) => _provider = provider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Migrate latest database changes during startup
        using var scope = _provider.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<OrdersContext>();

        // Here is the migration executed
        await dbContext.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
