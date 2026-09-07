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
        ReadOnlySpan<char> querySpan = query.AsSpan().TrimStart('?');
        foreach (Range range in querySpan.Split('&'))
        {
            ReadOnlySpan<char> component = querySpan[range];
            int equalsIndex = component.IndexOf('=');
            ReadOnlySpan<char> name = equalsIndex < 0 ? component : component[..equalsIndex];

            if (name.SequenceEqual("apiKey") ||
                (name.Contains('%') && string.Equals(Uri.UnescapeDataString(name.ToString()), "apiKey", StringComparison.Ordinal)))
                return true;
        }

        return false;
    }
}
