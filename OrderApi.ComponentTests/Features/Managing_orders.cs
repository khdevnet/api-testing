using System.Net;
using System.Threading.Tasks;
using LightBDD.XUnit2;
using OrderApi.ComponentTests.Features.Common;
using OrderApi.Core.Domain;

namespace Features;

public class Managing_orders : Base_feature
{
    [Scenario]
    public async Task Creating_order() =>
        await RunScenarioAsync<Managing_orders_steps>(
            s => s.Given_a_valid_user_account(),
            s => s.When_create_order_endpoint_is_called_for_products("product-A"),
            s => s.Then_response_should_have_status(HttpStatusCode.Created),
            s => s.Then_response_should_contain_order(),
            s => s.Then_OrderCreatedEvent_should_be_published(),
            s => s.Then_get_order_endpoint_should_return_order_with_status(OrderStatus.Created));

    [Scenario]
    public async Task Creating_order_for_invalid_account() =>
        await RunScenarioAsync<Managing_orders_steps>(
            s => s.Given_an_invalid_user_account(),
            s => s.When_create_order_endpoint_is_called_for_products("product-A"),
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
