using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Soenneker.Dtos.HttpClientOptions;
using Soenneker.Extensions.Configuration;
using Soenneker.CalCom.HttpClients.Abstract;
using Soenneker.Utils.HttpClientCache.Abstract;

namespace Soenneker.CalCom.HttpClients;

/// <inheritdoc cref="ICalComOpenApiHttpClient" />
public sealed class CalComOpenApiHttpClient : ICalComOpenApiHttpClient
{
    private readonly IHttpClientCache _httpClientCache;
    private readonly IConfiguration _config;

    private const string _prodBaseUrl = "https://api.cal.com/v1";

    public CalComOpenApiHttpClient(IHttpClientCache httpClientCache, IConfiguration config)
    {
        _httpClientCache = httpClientCache;
        _config = config;
    }

    public ValueTask<HttpClient> Get(CancellationToken cancellationToken = default)
    {
        return _httpClientCache.Get(nameof(CalComOpenApiHttpClient), (config: _config, baseUrl: _config["CalCom:ClientBaseUrl"] ?? _prodBaseUrl), static state =>
        {
            var apiKey = state.config.GetValueStrict<string>("CalCom:ApiKey");
            return new HttpClientOptions
            {
                BaseAddress = new Uri(state.baseUrl),
                DelegatingHandlerFactories = [() => new CalComApiKeyHandler(apiKey)]
            };
        }, cancellationToken);
    }

    public void Dispose()
    {
        _httpClientCache.RemoveSync(nameof(CalComOpenApiHttpClient));
    }

    public ValueTask DisposeAsync()
    {
        return _httpClientCache.Remove(nameof(CalComOpenApiHttpClient));
    }
}
