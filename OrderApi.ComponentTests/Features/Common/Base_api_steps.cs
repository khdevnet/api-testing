using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using LightBDD.Framework.Expectations;
using LightBDD.Framework.Formatting.Values;
using LightBDD.Framework.Parameters;
using RestEase;

namespace Features.Common;

internal abstract class Base_api_steps
{
    protected ApiException ApiException { get; set; } = null!;

    protected HttpResponseMessage Response { get; set; } = null!;

    public Task Then_response_should_have_status(Verifiable<HttpStatusCode> status)
    {
        status.SetActual(Response.StatusCode);
        return Task.CompletedTask;
    }

    public Task Then_response_is_bad_request()
    {
        var expactation = Expect.To.Equal(HttpStatusCode.BadRequest);
        expactation.Verify(Response.StatusCode, ValueFormattingServices.Current);
        return Task.CompletedTask;
    }

    public async Task Then_response_message_equal(string expectedMessage)
    {
        var actualMessage = await Response.Content.ReadAsStringAsync();

        actualMessage.Should().Be(expectedMessage);
    }

    public async Task Then_response_body_equal<TBody>(VerifiableTree expectedBody)
    {
        TBody actualBody = await GetResponseBody<TBody>();
        expectedBody.SetActual(actualBody);
    }

    public async Task Then_response_body_equal<TBody>(TBody expectedBody)
    {
        TBody actualBody = await GetResponseBody<TBody>();
        actualBody.Should().BeEquivalentTo(expectedBody);
    }

    public async Task Then_response_body_equal<TBody>(TBody expectedBody, Expression<Func<TBody, object>>? excludingExpression)
    {
        var actualBody = await GetResponseBody<TBody>();
        actualBody.Should().BeEquivalentTo(expectedBody, x => x.Excluding(excludingExpression));
    }

    public Task Then_response_is_no_content()
    {
        Response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        return Task.CompletedTask;
    }

    public Task Then_response_is_ok()
    {
        var expactation = Expect.To.Equal(HttpStatusCode.OK);
        expactation.Verify(Response.StatusCode, ValueFormattingServices.Current);

        return Task.CompletedTask;
    }

    private async Task<TBody?> GetResponseBody<TBody>()
    {
        var responseContent = await Response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<TBody>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
    }
}
