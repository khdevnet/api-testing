using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Core.Domain;
using OrderApi.Core.ExternalServices;
using OrderApi.Core.Messages;
using OrderApi.Core.Repositories;
using OrderApi.Models;
using SharedKernal;

namespace OrderApi.Controllers;

/// <summary>
/// Orders controller responsible for order creation and retrieval of order details.
/// </summary>
[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IAccountServiceClient _accountServiceClient;
    private readonly IBus _bus;
    private readonly IOrderRepository _repository;

    public OrdersController(IAccountServiceClient accountServiceClient, IBus bus, IOrderRepository repository)
    {
        _accountServiceClient = accountServiceClient;
        _bus = bus;
        _repository = repository;
    }

    /// <summary>
    /// Creates a new order and publishes OrderCreatedEvent
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        if (!await _accountServiceClient.IsValidAccount(request.AccountId))
        {
            return BadRequest("Invalid account");
        }
        var orderId = Guid.NewGuid();

        var order = new Order(orderId, request.AccountId)
            .AddProducts(request.Products);

        await _repository.AddAsync(order);

        // Outbox pattern should use there
        await _bus.Publish(new OrderCreatedEvent { OrderId = order.Id });

        return CreatedAtAction(nameof(GetById), new { orderId = order.Id }, order);
    }

    /// <summary>
    /// Retrieves order details.
    /// </summary>
    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetById(Guid orderId)
    {
        var order = await _repository.GetByIdAsync(orderId);

        if (order is null)
        {
            return NotFound();
        }

        var orderResponse = new GetOrderResponse
        {
            AccountId = order.AccountId,
            Products = order.Products.Select(p => p.Name).ToArray()
        };

        return Ok(orderResponse);
    }
}
