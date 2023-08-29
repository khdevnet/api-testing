namespace OrderApi.Infrastructure.ExternalServices.Sms;

public record SendSmsRequest(string phone, string Message);
