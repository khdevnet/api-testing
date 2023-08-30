using RestEase;

namespace OrderApi.Infrastructure.ExternalServices.Notifications.Sms;

[BasePath("providers/sms")]
public interface ISmsProvider
{
    [Post]
    Task<SendSmsResponse> Send([Body] SendSmsRequest request);
}
