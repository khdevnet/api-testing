using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Core.Domain;
using OrderApi.Core.Domain.UseCases;
using OrderApi.Core.Repositories;
using OrderApi.Models;

namespace OrderApi.Controllers;

/// <summary>
/// Orders controller responsible for order creation and retrieval of order details.
/// </summary>
[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _repository;
    private readonly CreateOrderUseCase _createOrderUseCase;

    public OrdersController(
        IOrderRepository repository,
        CreateOrderUseCase createOrderUseCase)
    {
        _repository = repository;
        _createOrderUseCase = createOrderUseCase;
    }

    /// <summary>
    /// Creates a new order and publishes OrderCreatedEvent
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        try
        {
            Order order = await _createOrderUseCase.CreateOrder(new CreateOrder(request.AccountId, request.Products));

            return CreatedAtAction(nameof(GetById), new { orderId = order.Id }, order);
        }
        catch (ApplicationException e)
        {
            return BadRequest(e.Message);
        }
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

        var orderResponse = new GetOrderResponse { AccountId = order.AccountId, OrderId = order.Id, Products = order.Products.Select(p => p.Name).ToArray() };

        return Ok(orderResponse);
    }
}
