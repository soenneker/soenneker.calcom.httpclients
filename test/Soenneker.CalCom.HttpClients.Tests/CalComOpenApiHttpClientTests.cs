using Soenneker.CalCom.HttpClients.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.CalCom.HttpClients.Tests;

[Collection("Collection")]
public sealed class CalComOpenApiHttpClientTests : FixturedUnitTest
{
    private readonly ICalComOpenApiHttpClient _httpclient;

    public CalComOpenApiHttpClientTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _httpclient = Resolve<ICalComOpenApiHttpClient>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
