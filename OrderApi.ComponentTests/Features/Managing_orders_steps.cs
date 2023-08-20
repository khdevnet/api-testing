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
using Moq;
using OrderApi.ComponentTests.Application.Infrastructure.AccountService;
using OrderApi.Core.ExternalServices.SmsService;

namespace Features;

internal class Managing_orders_steps : Base_api_steps, IDisposable
{
    private readonly AccountServiceMock _accountServiceMock;
    private readonly MessageListener _messageBusListener;
    private readonly IOrdersClient _client;
    private State<Guid> _accountId;
    private State<Guid> _orderId;

    private Order _order;

    public TestWebApplicationFactory App { get; }

    // Uses DI container to resolve these dependencies
    public Managing_orders_steps(
        TestWebApplicationFactory app)
    {
        _client = app.OrdersClient;
        _accountServiceMock = app.AccountClientMock;
        _messageBusListener = MessageListener.Start(app.MessageBusMock);
        App = app;
    }

    public Task Given_registered_user_account(InputTree<UserAccount> account)
    {
        _accountId = account.Input.AccountId;
        _accountServiceMock.SetupGetAccountRequest(account.Input);

        return Task.CompletedTask;
    }

    public Task Given_user_with_account_in_the_shop()
    {
        _accountId.Log();
        _accountServiceMock.SetupAccountValidateRequest(_accountId, true);

        return Task.CompletedTask;
    }

    public Task Given_sms_service_mock()
    {
        App.SmsServiceMock
            .Setup(m => m.SendOrderCreatedSms(It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        return Task.CompletedTask;
    }

    public Task Then_order_created_sms_sent_successful(string phoneNumber)
    {
        App.SmsServiceMock
            .Verify(m => m.SendOrderCreatedSms(It.Is<string>(arg => arg == phoneNumber)));

        return Task.CompletedTask;
    }

    public Task Given_not_exist_order_with_id_ORDERID(Guid orderId)
    {
        _orderId = orderId;

        App.OrderIdGeneratorMock.Setup(m => m.New())
            .Returns(orderId);

        return Task.CompletedTask;
    }

    public Task Given_an_invalid_user_account()
    {
        _accountId.Log();
        _accountServiceMock.SetupAccountValidateRequest(_accountId, false);

        return Task.CompletedTask;
    }

    public async Task When_user_send_create_new_order_request(params string[] products)
    {
        var request = new CreateOrderRequest { AccountId = _accountId, Products = products };
        Response = await _client.CreateOrder(request);
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
    {
        var expectedOrderCreatedEvent = new OrderCreatedEvent { OrderId = _orderId };
        expectedOrderCreatedEvent.Log();
        await _messageBusListener.EnsureReceived<OrderCreatedEvent>(x => x == expectedOrderCreatedEvent);
    }

    public async Task User_send_get_order_request()
        => Response = await _client.Get(_orderId);

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
                _ => When_user_send_create_new_order_request("OrderProduct-A", "OrderProduct-B", "OrderProduct-C"),
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
        => await _messageBusListener.EnsureReceived<OrderStatusUpdatedEvent>(x => x.OrderId == _order.Id && x.Status == status);

    public void Dispose()
        => _messageBusListener?.Dispose();
}
