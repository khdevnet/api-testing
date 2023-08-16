using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Vehicles.Infrastructure.Data;

public static class DataLayer
{
    public static IServiceCollection RegisterDataLayer(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<VehiclesContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("SchoolContext")));
        return serviceCollection;
    }
}