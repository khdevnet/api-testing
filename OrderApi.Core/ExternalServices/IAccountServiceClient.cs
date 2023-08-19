namespace OrderApi.Core.ExternalServices;

public interface IAccountServiceClient
{
    Task<bool> IsValidAccount(Guid accountId);
}
