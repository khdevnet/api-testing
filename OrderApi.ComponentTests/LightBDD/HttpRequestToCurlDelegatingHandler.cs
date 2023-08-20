using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HttpRequestToCurl;
using HttpRequestToCurl.Models;
using LightBDD.Framework;
using LightBDD.Framework.Reporting;

namespace OrderApi.ComponentTests.LightBDD;

public class HttpRequestToCurlDelegatingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string curlCommand = ToCurl(request);
        var requestShortPath = $"{request.Method}{request.RequestUri?.AbsolutePath?.Replace("/", "-")}-curl";
        await StepExecution.Current
            .AttachFile(m => m.CreateFromText(requestShortPath, "txt", curlCommand, Encoding.UTF8));

        var response = await base.SendAsync(request, cancellationToken);

        return response;
    }

    private static string ToCurl(HttpRequestMessage request)
    {
        var settings = new HttpRequestConverterSettings
        {
            AllowInsecureConnections = true,
            IgnoreSensitiveInformation = false
        };

        return HttpRequestConverter.ConvertToCurl(request, settings);
    }
}
