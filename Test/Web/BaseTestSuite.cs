
using Bootcamp.Drivers;
using Bootcamp.Config;
using Bootcamp.Model.Web.Pages;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using NUnit.Framework;

namespace Bootcamp.Test.JupiterPrimeUiTests;

[Parallelizable(scope: ParallelScope.Fixtures)]
[TestFixture]
public class BaseTestSuite
{
    protected IPage page;
    private static readonly IConfiguration Config = TestConfiguration.GetConfig();

    [SetUp]
    public async Task Setup()
    {
        var browser = Config.GetSection("Playwright:Browser").Value!;
        var channel = Config.GetSection("Playwright:Channel").Value!;
        var headless = bool.Parse(Config.GetSection("Playwright:Headless").Value!);
        var videoDir = Path.Combine(Config.GetSection("Playwright:TestResultsDirectory").Value!, "videos");
        var viewportSize = new ViewportSize
        {
            Width = int.Parse(Config.GetSection("Playwright:Viewport:Width").Value!),
            Height = int.Parse(Config.GetSection("Playwright:Viewport:Height").Value!)
        };

        page = IPageFactory.BuildPage(browser, channel, headless, videoDir, viewportSize);

        page.Context.SetDefaultNavigationTimeout(int.Parse(Config.GetSection("Playwright:DefaultNavigationTimeout").Value!));
        page.Context.SetDefaultTimeout(int.Parse(Config.GetSection("Playwright:DefaultTimeout").Value!));

        await page.Context.Tracing.StartAsync(new TracingStartOptions
        {
            Snapshots = true,
            Screenshots = true,
            Sources = true
        });
    }

    [TearDown]
    public async Task TearDown(){
        var testName = TestContext.CurrentContext.Test.FullName!;
        foreach (var invalidChar in Path.GetInvalidFileNameChars())
        {
            testName = testName.Replace(invalidChar, '_');
        }
        var testResultsDirectory = Path.GetFullPath(Config.GetSection("Playwright:TestResultsDirectory").Value!);
        var traceDirectory = Path.Combine(testResultsDirectory, "traces");
        Directory.CreateDirectory(traceDirectory);

        await page.Context.Tracing.StopAsync(new TracingStopOptions
        {
            Path = Path.Combine(traceDirectory, $"{testName}.zip")
        });
        await page.Context.CloseAsync();
        await page.Context.Browser!.DisposeAsync();
        await page.Context.DisposeAsync();
    }

    protected T Open<T>() where T : BasePage<T>
    {
        return ((BasePage<T>)typeof(T).GetConstructor([typeof(IPage)])!.Invoke([page])).Init()!;
    }
}
