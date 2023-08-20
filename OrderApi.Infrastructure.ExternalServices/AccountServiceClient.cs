using System.Net.Http.Json;
using OrderApi.Core.ExternalServices;
using OrderApi.Core.ExternalServices.AccountService;

namespace OrderApi.Infrastructure.ExternalServices;

internal class AccountServiceClient : IAccountServiceClient
{
    private readonly HttpClient _client;

    public AccountServiceClient(HttpClient client) => _client = client;

    public async Task<bool> IsValidAccount(Guid accountId)
        => await _client.GetFromJsonAsync<bool>($"/accounts/{accountId}/validate");

    public async Task<UserAccount> GetAccount(Guid accountId)
        => await _client.GetFromJsonAsync<UserAccount>($"/accounts/{accountId}");
}
