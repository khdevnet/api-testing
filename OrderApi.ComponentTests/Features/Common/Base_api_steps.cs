using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using LightBDD.Framework;
using LightBDD.Framework.Expectations;
using LightBDD.Framework.Formatting.Values;
using LightBDD.Framework.Parameters;
using RestEase;

namespace Features.Common;

internal abstract class Base_api_steps
{
    protected State<ApiException> ApiException { get; set; } = null!;

    protected State<HttpResponseMessage> Response { get; set; } = null!;

    public Task Then_response_should_have_status(Verifiable<HttpStatusCode> status)
    {
        status.SetActual(Response.GetValue().StatusCode);
        return Task.CompletedTask;
    }

    public async Task Then_response_message_equal(string expectedMessage)
    {
        var actualMessage = await Response.GetValue().Content.ReadAsStringAsync();

        actualMessage.Should().Be(expectedMessage);
    }

    public async Task Then_response_body_equal<TBody>(VerifiableTree expectedBody)
    {
        TBody actualBody = await GetResponseBody<TBody>();
        expectedBody.SetActual(actualBody);
    }

    private async Task<TBody?> GetResponseBody<TBody>()
    {
        var responseContent = await Response.GetValue().Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<TBody>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
    }
}
