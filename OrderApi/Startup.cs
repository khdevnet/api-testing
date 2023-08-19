using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OrderApi.Clients;
using OrderApi.Core;
using OrderApi.Handlers;
using OrderApi.Messages;
using Rebus.Config;
using Rebus.Persistence.FileSystem;
using Rebus.Routing.TypeBased;
using Rebus.Transport.FileSystem;
using OrderApi.Infrastructure.Data;

namespace OrderApi;

public class Startup
{
    public Startup(IConfiguration configuration)
        => Configuration = configuration;

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrdersService", Version = "v1" }));

        // using factory method to close LiteDatabase upon disposal
        services.AddHttpClient<AccountServiceClient>(cfg => cfg.BaseAddress = new Uri(Configuration["Clients:AccountService"]!));

        services.AddRebus(x => x
            .Transport(t => t.UseFileSystem(Configuration["Rebus:QueueDirectory"], "orders-queue"))
            .Subscriptions(t => t.UseJsonFile(Configuration["Rebus:SubscriptionFile"]))
            .Routing(r => r.TypeBased().MapAssemblyDerivedFrom<OrderCreatedEvent>("orders-queue")));
        services.AddRebusHandler<OrderStatusHandler>();


        services
            .RegisterCoreLayer()
            .RegisterDataLayer(Configuration);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrdersService v1"));
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
