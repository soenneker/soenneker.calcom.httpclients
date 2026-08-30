using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.CalCom.HttpClients;

internal sealed class CalComApiKeyHandler(string apiKey) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.RequestUri is { } requestUri && !HasApiKey(requestUri.Query))
        {
            var builder = new UriBuilder(requestUri);
            string encodedApiKey = Uri.EscapeDataString(apiKey);
            builder.Query = string.IsNullOrEmpty(builder.Query) ? $"apiKey={encodedApiKey}" : $"{builder.Query.TrimStart('?')}&apiKey={encodedApiKey}";
            request.RequestUri = builder.Uri;
        }

        return base.SendAsync(request, cancellationToken);
    }

    private static bool HasApiKey(string query)
    {
        foreach (string component in query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            string name = component.Split('=', 2)[0];

            if (string.Equals(Uri.UnescapeDataString(name), "apiKey", StringComparison.Ordinal))
                return true;
        }

        return false;
    }
}
