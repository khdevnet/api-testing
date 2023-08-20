using OrderApi.Core.ExternalServices.SmsService;
using RestEase;

namespace OrderApi.Infrastructure.ExternalServices.SmsService;

[BasePath("providers/sms")]
public interface ISmsProvider
{
    [Post]
    Task<SendSmsResponse> Send([Body] SendSmsRequest request);
}
