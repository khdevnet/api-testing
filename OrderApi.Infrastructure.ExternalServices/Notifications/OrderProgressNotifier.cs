using Microsoft.FeatureManagement;
using OrderApi.Core.ExternalServices.SmsService;
using OrderApi.Infrastructure.ExternalServices.Notifications.Sms;
using OrderApi.Infrastructure.ExternalServices.Notifications.Whatsup;

namespace OrderApi.Infrastructure.ExternalServices.Notifications;

public class OrderProgressNotifier : IOrderProgressNotifier
{
    private readonly ISmsProvider _smsProvider;
    private readonly IWhatsupProvider _whatsupProvider;
    private readonly IFeatureManager _featureManager;

    public OrderProgressNotifier(ISmsProvider smsProvider, IWhatsupProvider whatsupProvider, IFeatureManager featureManager)
    {
        _smsProvider = smsProvider;
        _whatsupProvider = whatsupProvider;
        _featureManager = featureManager;
    }

    public async Task SendOrderCreated(string phoneNumber)
    {
        if (await _featureManager.IsEnabledAsync("WhatsupProvider"))
        {
            await _whatsupProvider.Send(new MessageRequest(phoneNumber, "Order created successful."));
        }
        else
        {
            await _smsProvider.Send(new SendSmsRequest(phoneNumber, "Order created successful."));
        }
    }
}
