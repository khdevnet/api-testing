using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OrderApi.ComponentTests.Application.Clients;
using OrderApi.ComponentTests.Application.Infrastructure;
using OrderApi.ComponentTests.LightBDD;
using RestEase;
using Serilog;
using SharedKernal;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using OrderApi.ComponentTests.Application.Infrastructure.AccountService;
using OrderApi.Core;
using OrderApi.Core.ExternalServices.SmsService;
using OrderApi.Infrastructure.ExternalServices.Notifications.Sms;
using OrderApi.Infrastructure.ExternalServices.Notifications.Whatsup;

namespace OrderApi.ComponentTests.Application;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private const string EnvironmentName = "ComponentTests";
    private readonly TestAppConfigurations _testAppConfigurationsProvider;

    private readonly DelegatingHandler[] _requestHandlers =
    {
        new HttpRequestLogAsCommentDelegatingHandler(new LightBDDTestLogger<HttpRequestLogAsCommentDelegatingHandler>()), new HttpRequestToCurlDelegatingHandler()
    };

    public IOrdersClient OrdersClient { get; }
    public AccountServiceMock AccountClientMock { get; }
    public MessageBusMock MessageBusMock { get; }
    public Mock<ISmsProvider> SmsProviderMock { get; } = new();
    public Mock<IWhatsupProvider> WhatsupProviderMock { get; } = new();
    public Mock<IOrderIdGenerator> OrderIdGeneratorMock { get; } = new();

    public TestWebApplicationFactory(
        TestAppConfigurations testAppConfigurationsProvider,
        AccountServiceMock accountClientMock,
        MessageBusMock messageBusMock
    )
    {
        _testAppConfigurationsProvider = testAppConfigurationsProvider;
        MessageBusMock = messageBusMock;
        AccountClientMock = accountClientMock;
        OrdersClient = RestClient.For<IOrdersClient>(CreateClientWithLogger());

        SmsProviderMock
            .Setup(m => m.Send(It.IsAny<SendSmsRequest>()))
            .Returns(Task.FromResult(new SendSmsResponse(true)));

        WhatsupProviderMock
            .Setup(m => m.Send(It.IsAny<MessageRequest>()))
            .Returns(Task.CompletedTask);
    }

    public HttpClient CreateClientWithLogger()
    {
        try
        {
            return CreateDefaultClient(_requestHandlers);
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, nameof(CreateClientWithLogger));

            throw;
        }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.UseSerilog();

        builder.UseEnvironment(EnvironmentName);
        builder.UseContentRoot(Directory.GetCurrentDirectory());

        builder.ConfigureAppConfiguration(app =>
        {
            var appConfigurationOverrides = _testAppConfigurationsProvider.Get();

            if (appConfigurationOverrides.Any())
            {
                app.AddInMemoryCollection(appConfigurationOverrides);
            }
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<ISmsProvider>();
            services.AddSingleton<ISmsProvider>(_ => SmsProviderMock.Object);
            services.RemoveAll<IWhatsupProvider>();
            services.AddSingleton<IWhatsupProvider>(_ => WhatsupProviderMock.Object);
            services.RemoveAll<IOrderIdGenerator>();
            services.AddSingleton<IOrderIdGenerator>(_ => OrderIdGeneratorMock.Object);
            services.RemoveAll<IBus>();
            services.AddSingleton<IBus>(_ => MessageBusMock);
        });
    }
}
