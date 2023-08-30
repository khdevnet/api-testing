namespace OrderApi.Infrastructure.ExternalServices.Notifications.Sms;

public record SendSmsRequest(string phone, string Message);
