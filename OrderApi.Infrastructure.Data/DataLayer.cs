using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Core.Repositories;
using OrderApi.Infrastructure.Data.Repositories;

namespace Vehicles.Infrastructure.Data;

public static class DataLayer
{
    public static IServiceCollection RegisterDataLayer(this IServiceCollection serviceCollection, IConfiguration configuration)
    {

        serviceCollection.AddScoped<IOrderRepository, OrderRepository>();
        serviceCollection.AddDbContext<OrdersContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("SchoolContext")));
        return serviceCollection;
    }
}