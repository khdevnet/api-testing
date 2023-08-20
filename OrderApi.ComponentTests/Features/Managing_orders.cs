using System.Net;
using System.Threading.Tasks;
using LightBDD.Framework.Parameters;
using LightBDD.XUnit2;
using OrderApi.ComponentTests.Features.Common;
using OrderApi.Core.Domain;
using OrderApi.Models;

namespace Features;

public class Managing_orders : Base_feature
{
    [Scenario]
    public async Task Creating_order()
    {
        await RunScenarioAsync<Managing_orders_steps>(
            s => s.Given_user_with_account_in_the_shop(),
            s => s.User_send_create_new_order_with_products_request("product-A"),
            s => s.Then_response_should_have_status(HttpStatusCode.Created),
            s => s.User_send_get_order_request(),
            s => s.Then_response_should_have_status(HttpStatusCode.OK),
            s => s.Then_response_body_equal<GetOrderResponse>(
                Tree.ExpectContaining(
                    new
                    {
                        Products = new[] { "product-A" }
                    }
                )
            ),
            s => s.Then_OrderCreatedEvent_should_be_published());
    }

    [Scenario]
    public async Task Creating_order_for_invalid_account() =>
        await RunScenarioAsync<Managing_orders_steps>(
            s => s.Given_an_invalid_user_account(),
            s => s.User_send_create_new_order_with_products_request("product-A"),
            s => s.Then_response_should_have_status(HttpStatusCode.BadRequest));

    [Scenario]
    public async Task Rejecting_order() =>
        await RunScenarioAsync<Managing_orders_steps>(
            s => s.Given_a_created_order(),
            s => s.When_RejectOrderCommand_is_sent_for_this_order(),
            s => s.Then_OrderStatusUpdatedEvent_should_be_published_with_status(OrderStatus.Rejected),
            s => s.Then_get_order_endpoint_should_return_order_with_status(OrderStatus.Rejected));

    [Scenario]
    public async Task Approving_order() =>
        await RunScenarioAsync<Managing_orders_steps>(
            s => s.Given_a_created_order(),
            s => s.When_ApproveOrderCommand_is_sent_for_this_order(),
            s => s.Then_OrderStatusUpdatedEvent_should_be_published_with_status(OrderStatus.Complete),
            s => s.Then_get_order_endpoint_should_return_order_with_status(OrderStatus.Complete));

    [Scenario]
    public async Task Dispatching_products_for_approved_order() =>
        await RunScenarioAsync<Managing_orders_steps>(
            s => s.Given_a_created_order(),
            s => s.When_ApproveOrderCommand_is_sent_for_this_order(),
            s => s.Then_OrderStatusUpdatedEvent_should_be_published_with_status(OrderStatus.Complete));
}
