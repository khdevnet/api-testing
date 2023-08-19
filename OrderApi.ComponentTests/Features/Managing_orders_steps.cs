using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using LightBDD.Framework;
using LightBDD.Framework.Messaging;
using LightBDD.Framework.Parameters;
using LightBDD.Framework.Scenarios;
using OrderApi.ComponentTests.Application.Clients;
using OrderApi.ComponentTests.Application;
using OrderApi.Core.Domain;
using OrderApi.Models;
using Xunit;
using OrderApi.ComponentTests.Application.Infrastructure;
using OrderApi.Core.Messages;
using SharedKernal;
using OrderApi.Messages;

namespace Features;

internal class Managing_orders_steps : IDisposable
{
    private readonly AccountServiceMock _accountService;
    private readonly MessageListener _listener;
    private readonly IOrdersClient _client;
    private readonly Guid _accountId = Guid.NewGuid();
    private HttpResponseMessage _response;
    private Order _order;

    public TestWebApplicationFactory App { get; }

    // Uses DI container to resolve these dependencies
    public Managing_orders_steps(
        TestWebApplicationFactory app,
        MessageBusMock messageBusMock)
    {
        _client = app.OrdersClient;
        _accountService = app.AccountClientMock;
        _listener = MessageListener.Start(messageBusMock);
        App = app;
    }

    public Task Given_a_valid_account()
    {
        _accountService.SetupGetAccount(_accountId, true);
        return Task.CompletedTask;
    }

    public Task Given_an_invalid_account()
    {
        _accountService.SetupGetAccount(_accountId, false);
        return Task.CompletedTask;
    }

    public async Task When_create_order_endpoint_is_called_for_products(params string[] products)
    {
        var request = new CreateOrderRequest { AccountId = _accountId, Products = products };
        _response = await _client.CreateOrder(request);
    }

    public Task Then_response_should_have_status(Verifiable<HttpStatusCode> status)
    {
        status.SetActual(_response.StatusCode);
        return Task.CompletedTask;
    }

    public async Task Then_response_should_contain_order()
    {
        _order = await _response.Content.ReadFromJsonAsync<Order>();
        Assert.NotEqual(Guid.Empty, _order?.Id);
    }

    public async Task Then_OrderCreatedEvent_should_be_published()
        => await _listener.EnsureReceived<OrderCreatedEvent>(x => x.OrderId == _order.Id);

    public async Task Then_get_order_endpoint_should_return_order_with_status(Verifiable<OrderStatus> status)
    {
        var orderResponse = await _client.Get(_order.Id);
        var order = await orderResponse.Content.ReadFromJsonAsync<Order>();
        status.SetActual(order!.Status);
    }

    public Task<CompositeStep> Given_a_created_order() =>
        Task.FromResult(CompositeStep.DefineNew()
            .AddAsyncSteps(
                _ => Given_a_valid_account(),
                _ => When_create_order_endpoint_is_called_for_products("OrderProduct-A", "OrderProduct-B", "OrderProduct-C"),
                _ => Then_response_should_have_status(HttpStatusCode.Created),
                _ => Then_response_should_contain_order())
            .Build());

    public async Task When_RejectOrderCommand_is_sent_for_this_order()
    {
        var handler = App.RetrieveScopedService<IHandleMessages<RejectOrderCommand>>();
        await handler.Handle(new RejectOrderCommand { OrderId = _order.Id });
    }

    public async Task When_ApproveOrderCommand_is_sent_for_this_order()
    {
        var handler = App.RetrieveScopedService<IHandleMessages<ApproveOrderCommand>>();
        await handler.Handle(new ApproveOrderCommand { OrderId = _order.Id });
    }

    public async Task Then_OrderStatusUpdatedEvent_should_be_published_with_status(OrderStatus status)
        => await _listener.EnsureReceived<OrderStatusUpdatedEvent>(x => x.OrderId == _order.Id && x.Status == status);

    public void Dispose()
        => _listener?.Dispose();
}
