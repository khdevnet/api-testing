using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Clients;
using OrderApi.Core.Domain;
using OrderApi.Core.Repositories;
using OrderApi.Messages;
using OrderApi.Models;
using Rebus.Bus;

namespace OrderApi.Controllers;

/// <summary>
/// Orders controller responsible for order creation and retrieval of order details.
/// </summary>
[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly AccountServiceClient _accountServiceClient;
    private readonly IBus _bus;
    private readonly IOrderRepository _repository;

    public OrdersController(AccountServiceClient accountServiceClient, IBus bus, IOrderRepository repository)
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

        await _bus.Publish(new OrderCreatedEvent { OrderId = order.Id });

        return CreatedAtAction("GetById", new { orderId = order.Id }, order);
    }

    /// <summary>
    /// Retrieves order details.
    /// </summary>
    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetById(Guid orderId)
    {
        var order = await _repository.GetByIdAsync(orderId);
        return order != null ? Ok(order) : NotFound();
    }
}
