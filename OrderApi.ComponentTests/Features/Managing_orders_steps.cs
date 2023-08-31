using System;
using System.Net;
using System.Threading.Tasks;
using LightBDD.Framework;
using LightBDD.Framework.Messaging;
using LightBDD.Framework.Parameters;
using LightBDD.Framework.Scenarios;
using OrderApi.ComponentTests.Application.Clients;
using OrderApi.ComponentTests.Application;
using OrderApi.Models;
using OrderApi.Core.Messages;
using SharedKernal;
using OrderApi.Messages;
using OrderApi.ComponentTests.LightBDD;
using Features.Common;
using FluentAssertions;
using Moq;
using OrderApi.ComponentTests.Application.Infrastructure.AccountService;
using OrderApi.Infrastructure.ExternalServices.Notifications.Sms;
using OrderApi.Infrastructure.ExternalServices.Notifications.Whatsup;

namespace Features;

internal class Managing_orders_steps : Base_api_steps, IDisposable
{
    private readonly AccountServiceMock _accountServiceMock;
    private readonly MessageListener _messageBusListener;
    private readonly IOrdersClient _client;
    private State<Guid> _accountId;
    private State<Guid> _orderId;

    public Managing_orders_steps(
        TestWebApplicationFactory app)
    {
        _client = app.OrdersClient;
        _accountServiceMock = app.AccountClientMock;
        _messageBusListener = MessageListener.Start(app.MessageBusMock);
        App = app;
    }

    public TestWebApplicationFactory App { get; }

    public Task Given_registered_user_account(InputTree<UserAccount> account)
    {
        _accountId = account.Input.AccountId;
        _accountServiceMock.SetupGetAccountRequest(account.Input);

        return Task.CompletedTask;
    }

    public Task Then_order_created_sms_sent(string phoneNumber)
    {
        App.SmsProviderMock
            .Verify(m => m.Send(It.Is<SendSmsRequest>(arg => arg.phone == phoneNumber)));

        return Task.CompletedTask;
    }

    public Task Then_order_created_whatsup_message_sent(string phoneNumber)
    {
        App.WhatsupProviderMock
            .Verify(m => m.Send(It.Is<MessageRequest>(arg => arg.PhoneNumber == phoneNumber)));

        return Task.CompletedTask;
    }

    public Task Then_order_created_sms_not_sent(string phoneNumber)
    {
        App.SmsProviderMock
            .Verify(m => m.Send(It.Is<SendSmsRequest>(arg => arg.phone == phoneNumber)), Times.Never);

        return Task.CompletedTask;
    }

    public Task Given_not_exist_order_with_id_ORDERID(Guid orderId)
    {
        _orderId = orderId;

        App.OrderIdGeneratorMock.Setup(m => m.New())
            .Returns(orderId);

        return Task.CompletedTask;
    }

    public async Task When_user_send_create_new_order_request(params string[] products)
    {
        var request = new CreateOrderRequest { AccountId = _accountId, Products = products };
        Response = await _client.CreateOrder(request);
    }

    public async Task Then_OrderCreatedEvent_should_be_published()
    {
        var orderCreatedEvent = new OrderCreatedEvent { OrderId = _orderId };
        orderCreatedEvent.Log();
        await _messageBusListener.EnsureReceived<OrderCreatedEvent>(x => x == orderCreatedEvent);
    }

    public Task Then_OrderCreatedEvent_should_not_be_published()
    {
        var events = _messageBusListener
            .GetMessages<OrderCreatedEvent>();

        events.Should().BeEmpty();

        return Task.CompletedTask;
    }

    public async Task When_user_get_order_details_by_send_get_order_request()
        => Response = await _client.Get(_orderId);

    public async Task<CompositeStep> Given_a_created_order_precondition(Guid orderId)
        => await Given_a_created_order_scenario(orderId);

    public Task<CompositeStep> Given_a_created_order_scenario(Guid orderId) =>
        Task.FromResult(CompositeStep.DefineNew()
            .AddAsyncSteps(
                _ => Given_not_exist_order_with_id_ORDERID(orderId),
                _ => Given_registered_user_account(TestAccounts.JohnDoe),
                _ => When_user_send_create_new_order_request("OrderProduct-A", "OrderProduct-B", "OrderProduct-C"),
                _ => Then_response_should_have_status(HttpStatusCode.Created))
            .Build());

    public async Task When_RejectOrderCommand_is_received_for_this_order()
    {
        var handler = App.RetrieveScopedService<IHandleMessages<RejectOrderCommand>>();
        await handler.Handle(new RejectOrderCommand { OrderId = _orderId });
    }

    public async Task When_ApproveOrderCommand_received()
    {
        var handler = App.RetrieveScopedService<IHandleMessages<ApproveOrderCommand>>();
        await handler.Handle(new ApproveOrderCommand { OrderId = _orderId });
    }

    public void Dispose()
        => _messageBusListener?.Dispose();
}
