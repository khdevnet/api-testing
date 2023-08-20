using OrderApi.Core.ExternalServices.SmsService;

namespace OrderApi.Infrastructure.ExternalServices.SmsService;

public class SmsService : ISmsService
{
    private readonly ISmsProvider _smsProvider;

    public SmsService(ISmsProvider smsProvider)
        => _smsProvider = smsProvider;

    public Task SendOrderCreatedSms(string phoneNumber)
        => _smsProvider.Send(new SendSmsRequest(phoneNumber, "Order created successful."));
}
