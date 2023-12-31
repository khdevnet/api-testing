﻿using OrderApi.Core.Domain;
using OrderApi.Core.ExternalServices.AccountService;
using OrderApi.Core.ExternalServices.SmsService;
using OrderApi.Core.Messages;
using OrderApi.Core.Repositories;
using SharedKernal;

namespace OrderApi.Core.Services;

public class CreateOrderService
{
    private readonly IAccountServiceClient _accountServiceClient;
    private readonly IOrderProgressNotifier _orderProgressNotifier;
    private readonly IBus _bus;
    private readonly IOrderRepository _repository;
    private readonly IOrderIdGenerator _orderIdGenerator;

    public CreateOrderService(
        IAccountServiceClient accountServiceClient,
        IOrderProgressNotifier orderProgressNotifier,
        IBus bus,
        IOrderRepository repository,
        IOrderIdGenerator orderIdGenerator)
    {
        _accountServiceClient = accountServiceClient;
        _orderProgressNotifier = orderProgressNotifier;
        _bus = bus;
        _repository = repository;
        _orderIdGenerator = orderIdGenerator;
    }

    public async Task<Order> CreateOrder(CreateOrder request)
    {
        var userAccount = await _accountServiceClient.GetAccount(request.AccountId);

        if (!userAccount.Approved)
        {
            throw new ApplicationException("Invalid account");
        }

        var orderId = _orderIdGenerator.New();

        var order = new Order(orderId, request.AccountId)
            .AddProducts(request.Products);

        await _repository.AddAsync(order);

        // Outbox pattern should use there
        await _bus.Publish(new OrderCreatedEvent { OrderId = order.Id });
        await _orderProgressNotifier.SendOrderCreated(userAccount.PhoneNumber);

        return order;
    }
}
