namespace OrderApi.Core.ExternalServices.SmsService;

public interface ISmsService
{
    Task SendOrderCreatedSms(string phoneNumber);
}
