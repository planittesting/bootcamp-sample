using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using Bootcamp.Config;

namespace Bootcamp.Test.SwaggerPetStoreApiTests;

[Parallelizable(scope: ParallelScope.Fixtures)]
[TestFixture]
public class BaseApiTests {

    protected string baseUrl;

    private static readonly IConfiguration config = TestConfiguration.GetConfig();

    [SetUp]
    public void Setup() => baseUrl = config.GetSection("API_BASEURL").Value!;
}
