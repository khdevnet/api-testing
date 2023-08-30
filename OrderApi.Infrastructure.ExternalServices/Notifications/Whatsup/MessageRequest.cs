namespace OrderApi.Infrastructure.ExternalServices.Notifications.Whatsup;

public record MessageRequest(string PhoneNumber, string Message);
