using OrderApi.Core.ExternalServices.AccountService;
using OrderApi.Core.ExternalServices.SmsService;
using OrderApi.Core.Messages;
using OrderApi.Core.Repositories;
using SharedKernal;

namespace OrderApi.Core.Domain.UseCases;

public class CreateOrderUseCase
{
    private readonly IAccountServiceClient _accountServiceClient;
    private readonly ISmsService _smsService;
    private readonly IBus _bus;
    private readonly IOrderRepository _repository;
    private readonly IOrderIdGenerator _orderIdGenerator;

    public CreateOrderUseCase(
        IAccountServiceClient accountServiceClient,
        ISmsService smsService,
        IBus bus,
        IOrderRepository repository,
        IOrderIdGenerator orderIdGenerator)
    {
        _accountServiceClient = accountServiceClient;
        _smsService = smsService;
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
        await _smsService.SendOrderCreatedSms(userAccount.PhoneNumber);

        return order;
    }
}
