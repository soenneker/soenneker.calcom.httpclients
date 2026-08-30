[![](https://img.shields.io/nuget/v/soenneker.calcom.httpclients.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.calcom.httpclients/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.calcom.httpclients/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.calcom.httpclients/actions/workflows/publish-package.yml)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.calcom.httpclients/codeql.yml?style=for-the-badge&label=codeql)](https://github.com/soenneker/soenneker.calcom.httpclients/actions/workflows/codeql.yml)
[![](https://img.shields.io/nuget/dt/soenneker.calcom.httpclients.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.calcom.httpclients/)

# Soenneker.CalCom.HttpClients

A cached `HttpClient` for Cal.com's v1 API that applies the required API key query parameter to every request.

## Installation

```bash
dotnet add package Soenneker.CalCom.HttpClients
```

## Configuration

```json
{
  "CalCom": {
    "ApiKey": "your-api-key"
  }
}
```

`CalCom:ApiKey` is required. Set `CalCom:ClientBaseUrl` only when requests should use a compatible proxy or test server instead of `https://api.cal.com/v1`.

## Registration and usage

```csharp
using Soenneker.CalCom.HttpClients.Abstract;
using Soenneker.CalCom.HttpClients.Registrars;

services.AddCalComOpenApiHttpClientAsSingleton();

public sealed class CalComService(ICalComOpenApiHttpClient clientProvider)
{
    public async Task<string> GetUsers(CancellationToken cancellationToken)
    {
        HttpClient client = await clientProvider.Get(cancellationToken);
        return await client.GetStringAsync("users", cancellationToken);
    }
}
```

The provider owns its named cache entry. Disposing the provider removes that entry and disposes the cached client. Prefer singleton registration for normal application-wide use; scoped registration creates a scoped owner for the same named entry and should only be used when that lifetime is intentional.

Do not put the API key into request URLs yourself. The client adds `apiKey` at send time and preserves any existing query parameters.
