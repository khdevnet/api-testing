using System;
using System.Net;
using System.Threading.Tasks;
using LightBDD.Core.Dependencies;
using LightBDD.Framework.Parameters;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;
using OrderApi.ComponentTests.Application;
using OrderApi.ComponentTests.Application.Infrastructure;
using OrderApi.ComponentTests.Application.Infrastructure.AccountService;
using OrderApi.ComponentTests.Features.Common;
using OrderApi.Core.Domain;
using OrderApi.Models;

namespace Features;

public class Managing_orders : Base_feature
{
    [Scenario]
    public async Task Creating_order()
    {
        var orderId = Guid.NewGuid();

        await RunScenarioAsync<Managing_orders_steps>(
            s => s.Given_not_exist_order_with_id_ORDERID(orderId),
            s => s.Given_registered_user_account(TestAccounts.JohnDoe),
            s => s.When_user_send_create_new_order_request("product-A"),
            s => s.Then_response_should_have_status(HttpStatusCode.Created),
            s => s.When_user_get_order_details_by_send_get_order_request(),
            s => s.Then_response_should_have_status(HttpStatusCode.OK),
            s => s.Then_response_body_equal<GetOrderResponse>(
                Tree.ExpectContaining(
                    new { OrderId = orderId, TestAccounts.JohnDoe.AccountId, Products = new[] { "product-A" } }
                )
            ),
            s => s.Then_OrderCreatedEvent_should_be_published(),
            s => s.Then_order_created_sms_sent(TestAccounts.JohnDoe.PhoneNumber));
    }

    [Scenario]
    public async Task Creating_order_with_whatsup_confirmation_feature_on()
    {
        var orderId = Guid.NewGuid();

        await Runner.WithContext(
                r => new Managing_orders_steps(
                    new TestWebApplicationFactory(
                        r.Resolve<FeatureTestAppConfigurations>(),
                        r.Resolve<AccountServiceMock>(),
                        r.Resolve<MessageBusMock>())
                )
            )
            .RunScenarioAsync(
                s => s.Given_not_exist_order_with_id_ORDERID(orderId),
                s => s.Given_registered_user_account(TestAccounts.JohnDoe),
                s => s.When_user_send_create_new_order_request("product-A"),
                s => s.Then_response_should_have_status(HttpStatusCode.Created),
                s => s.When_user_get_order_details_by_send_get_order_request(),
                s => s.Then_response_should_have_status(HttpStatusCode.OK),
                s => s.Then_response_body_equal<GetOrderResponse>(
                    Tree.ExpectContaining(
                        new { OrderId = orderId, TestAccounts.JohnDoe.AccountId, Products = new[] { "product-A" } }
                    )
                ),
                s => s.Then_OrderCreatedEvent_should_be_published(),
                s => s.Then_order_created_whatsup_message_sent(TestAccounts.JohnDoe.PhoneNumber),
                s => s.Then_order_created_sms_not_sent(TestAccounts.JohnDoe.PhoneNumber));
    }

    [Scenario]
    public async Task Creating_order_for_not_approved_account()
    {
        var orderId = Guid.NewGuid();
        var notApprovedUserAccount = TestAccounts.JohnDoe with { Approved = false };

        await RunScenarioAsync<Managing_orders_steps>(
            s => s.Given_not_exist_order_with_id_ORDERID(orderId),
            s => s.Given_registered_user_account(notApprovedUserAccount),
            s => s.When_user_send_create_new_order_request("product-A"),
            s => s.Then_response_should_have_status(HttpStatusCode.BadRequest),
            s => s.When_user_get_order_details_by_send_get_order_request(),
            s => s.Then_response_should_have_status(HttpStatusCode.NotFound),
            s => s.Then_OrderCreatedEvent_should_not_be_published(),
            s => s.Then_order_created_sms_not_sent(TestAccounts.JohnDoe.PhoneNumber));
    }

    [Scenario]
    public async Task Rejecting_order()
    {
        var orderId = Guid.NewGuid();

        await RunScenarioAsync<Managing_orders_steps>(
            s => s.Given_a_created_order_precondition(orderId),
            s => s.When_RejectOrderCommand_is_received_for_this_order(),
            s => s.When_user_get_order_details_by_send_get_order_request(),
            s => s.Then_response_body_equal<GetOrderResponse>(
                Tree.ExpectContaining(
                    new { OrderId = orderId, Status = OrderStatus.Rejected, TestAccounts.JohnDoe.AccountId }
                )
            ));
    }

    [Scenario]
    public async Task Approving_order()
    {
        var orderId = Guid.NewGuid();

        await RunScenarioAsync<Managing_orders_steps>(
            s => s.Given_a_created_order_scenario(orderId),
            s => s.When_ApproveOrderCommand_received(),
            s => s.When_user_get_order_details_by_send_get_order_request(),
            s => s.Then_response_body_equal<GetOrderResponse>(
                Tree.ExpectContaining(
                    new { OrderId = orderId, Status = OrderStatus.Complete, TestAccounts.JohnDoe.AccountId }
                )
            ));
    }
    //
    // protected override Managing_orders_steps CreateFeatureContext<Managing_orders_steps>(IDependencyResolver resolver)
    // {
    //     return new Managing_orders_steps();
    // }
}
