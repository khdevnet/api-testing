namespace OrderApi.Core.ExternalServices.AccountService;

public interface IAccountServiceClient
{
    Task<bool> IsValidAccount(Guid accountId);
    Task<UserAccount> GetAccount(Guid accountId);
}
