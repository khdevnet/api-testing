namespace OrderApi.Infrastructure.ExternalServices.Notifications.Whatsup;

public class WhatsupProvider : IWhatsupProvider
{
    public Task Send(MessageRequest message) => throw new NotImplementedException();
}
