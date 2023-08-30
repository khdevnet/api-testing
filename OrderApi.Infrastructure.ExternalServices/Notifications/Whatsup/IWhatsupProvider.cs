namespace OrderApi.Infrastructure.ExternalServices.Notifications.Whatsup;

public interface IWhatsupProvider
{
    Task Send(MessageRequest message);
}
