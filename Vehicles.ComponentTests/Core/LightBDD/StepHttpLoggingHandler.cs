using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Vehicles.ComponentTests.Core.LightBDD;

public class StepHttpLoggingHandler : DelegatingHandler
{
    private readonly ILogger<StepHttpLoggingHandler> _logger;

    public StepHttpLoggingHandler(ILogger<StepHttpLoggingHandler> logger)
        => _logger = logger;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var requestShortPath = $"{request.Method} {request.RequestUri?.AbsolutePath}";

        await LogRequestAsync($"{requestShortPath}", request);

        var response = await base.SendAsync(request, cancellationToken);

        await LogResponseAsync($"Response: ", response);

        return response;
    }

    private static string GetHeaders(HttpHeaders headers)
    {
        var properties = headers.ToDictionary(h => h.Key, h => h.Value);
        return JsonConvert.SerializeObject(properties, Formatting.Indented);
    }

    private static async Task<string> GetBodyAsync(HttpContent? content)
        => content == null ? string.Empty : await content.ReadAsStringAsync();

    private async Task LogRequestAsync(string message, HttpRequestMessage request)
    {
        var body = await GetBodyAsync(request.Content);
        var properties = new Dictionary<string, string>() {
            { "Url", request.RequestUri?.ToString() ?? string.Empty },
            { "Method", request.Method.ToString() },
            { "Headers", GetHeaders(request.Headers) },
            { "Body", body },
        };

        LogInformation(message, properties);
    }

    private void LogInformation(string message, Dictionary<string, string> properties)
        => _logger.LogInformation($"{message}: {Environment.NewLine} {JsonConvert.SerializeObject(properties, Formatting.Indented)}");

    private async Task LogResponseAsync(string message, HttpResponseMessage response)
    {
        var body = await GetBodyAsync(response.Content);
        var properties = new Dictionary<string, string>()
        {
            { "Headers", GetHeaders(response.Headers) },
            { "StatusCode", response.StatusCode.ToString() },
            { "IsSuccessStatusCode", response.IsSuccessStatusCode.ToString() },
            { "ReasonPhrase", response.ReasonPhrase ?? string.Empty },
            { "ResponseBody", body },
        };

        LogInformation(message, properties);
    }
}
