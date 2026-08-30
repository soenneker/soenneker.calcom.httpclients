using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace Soenneker.CalCom.HttpClients.Abstract;

/// <summary>
/// Provides a cached <see cref="HttpClient"/> configured for Cal.com's v1 API.
/// </summary>
public interface ICalComOpenApiHttpClient: IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets the configured client. The API key is added to each request as the required <c>apiKey</c> query parameter.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the result of the operation.</returns>
    ValueTask<HttpClient> Get(CancellationToken cancellationToken = default);
}
