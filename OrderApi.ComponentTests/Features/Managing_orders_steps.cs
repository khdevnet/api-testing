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
using OrderApi.ComponentTests.LightBDD;
using Features.Common;

namespace Features;

internal class Managing_orders_steps : Base_api_steps, IDisposable
{
    private readonly AccountServiceMock _accountService;
    private readonly MessageListener _listener;
    private readonly IOrdersClient _client;
    private readonly Guid _accountId = Guid.NewGuid();
    private Order _order;

    public TestWebApplicationFactory App { get; }

    // Uses DI container to resolve these dependencies
    public Managing_orders_steps(
        TestWebApplicationFactory app)
    {
        _client = app.OrdersClient;
        _accountService = app.AccountClientMock;
        _listener = MessageListener.Start(app.MessageBusMock);
        App = app;
    }

    public Task Given_user_with_account_in_the_shop()
    {
        _accountId.Log();
        _accountService.SetupGetAccount(_accountId, true);
        return Task.CompletedTask;
    }

    public Task Given_an_invalid_user_account()
    {
        _accountId.Log();
        _accountService.SetupGetAccount(_accountId, false);
        return Task.CompletedTask;
    }

    public async Task User_send_create_new_order_with_products_request(params string[] products)
    {
        var request = new CreateOrderRequest { AccountId = _accountId, Products = products };

        Response = await _client.CreateOrder(request);
        _order = await Response.Content.ReadFromJsonAsync<Order>();
    }

    public async Task Then_response_should_contain_order(VerifiableTree verifiableOrder)
    {
        var or = await Response.Content.ReadFromJsonAsync<GetOrderResponse>();
        verifiableOrder.SetActual(_order);
        //_order.Should()
        //    .BeEquivalentTo(
        //        VerifiableOrder.GetActual(),
        //        options => options.Excluding(o => o.Id));
    }

    public async Task Then_response_should_contain_order()
    {
        _order = await Response.Content.ReadFromJsonAsync<Order>();
        Assert.NotEqual(Guid.Empty, _order?.Id);
    }

    public async Task Then_OrderCreatedEvent_should_be_published()
        => await _listener.EnsureReceived<OrderCreatedEvent>(x => x.OrderId == _order.Id);

    public async Task User_send_get_order_request()
        => Response = await _client.Get(_order.Id);

    public async Task Then_get_order_endpoint_should_return_order_with_status(Verifiable<OrderStatus> status)
    {
        var orderResponse = await _client.Get(_order.Id);
        var order = await orderResponse.Content.ReadFromJsonAsync<Order>();
        status.SetActual(order!.Status);
    }

    public Task<CompositeStep> Given_a_created_order() =>
        Task.FromResult(CompositeStep.DefineNew()
            .AddAsyncSteps(
                _ => Given_user_with_account_in_the_shop(),
                _ => User_send_create_new_order_with_products_request("OrderProduct-A", "OrderProduct-B", "OrderProduct-C"),
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
