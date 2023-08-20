namespace OrderApi.Core.ExternalServices.SmsService;

public record SendSmsRequest(string phone, string Message);
