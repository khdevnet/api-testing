using System;
using System.Net.Http;
using System.Threading.Tasks;
using OrderApi.Models;
using RestEase;

namespace OrderApi.ComponentTests.Clients;

[BasePath("orders")]
[AllowAnyStatusCode]
public interface IOrdersClient
{
    [Post()]
    Task<HttpResponseMessage> CreateOrder([Body] CreateOrderRequest orderRequest);

    [Get("{orderId}")]
    Task<HttpResponseMessage> Get([Path] Guid orderId);
}
