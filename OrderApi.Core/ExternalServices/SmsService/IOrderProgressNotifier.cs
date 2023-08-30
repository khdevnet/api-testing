namespace OrderApi.Core.ExternalServices.SmsService;

public interface IOrderProgressNotifier
{
    Task SendOrderCreated(string phoneNumber);
}
