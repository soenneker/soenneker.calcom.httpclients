using Soenneker.CalCom.HttpClients.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.CalCom.HttpClients.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class CalComOpenApiHttpClientTests : HostedUnitTest
{
    private readonly ICalComOpenApiHttpClient _httpclient;

    public CalComOpenApiHttpClientTests(Host host) : base(host)
    {
        _httpclient = Resolve<ICalComOpenApiHttpClient>(true);
    }

    [Test]
    public void Default()
    {

    }
}
