using NUnit.Framework;
using Microsoft.Playwright;
using System.Collections;
using Microsoft.Extensions.Configuration;
using Reqnroll.BoDi;
using Reqnroll.Infrastructure;
using Bootcamp.Config;
using Bootcamp.Drivers;
[assembly: Parallelizable(ParallelScope.Fixtures), LevelOfParallelism(2)]

namespace Bootcamp.BDD.Hooks;
[Binding]
public class TestHooks
{
    private readonly IObjectContainer _objectContainer;
    private readonly FeatureContext _featureContext;
    private readonly ScenarioContext _scenarioContext;
    private readonly ReqnrollOutputHelper _reqnrollOutputHelper;
    private static IConfiguration Config;

    public TestHooks(IObjectContainer objectContainer, FeatureContext featureContext, ScenarioContext scenarioContext, ReqnrollOutputHelper reqnrollOutputHelper)
    {
        _objectContainer = objectContainer;
        _featureContext = featureContext;
        _scenarioContext = scenarioContext;
        _reqnrollOutputHelper = reqnrollOutputHelper;
        Config = TestConfiguration.GetConfig();
    }

    [BeforeScenario("@web")]
    public async Task Setup()
    {
        var browser = Config.GetSection("Playwright:Browser").Value;
        var headless = bool.Parse(Config.GetSection("Playwright:Headless").Value!);
        var videoDir = Path.Combine(Config.GetSection("Playwright:TestResultsDirectory").Value!, "videos");        
        var channel = Config.GetSection("Playwright:Channel").Value;

        ViewportSize viewportSize = new ViewportSize()
        {
            Width = int.Parse(Config.GetSection("Playwright:Viewport:Width").Value!),
            Height = int.Parse(Config.GetSection("Playwright:Viewport:Height").Value!)
        };

        var page = IPageFactory.BuildPage(browser!, channel!, headless, videoDir, viewportSize);

        await page.Context.Tracing.StartAsync(new TracingStartOptions
        {
            Snapshots = true,
            Screenshots = true,
            Sources = true
        });

        page.Context.SetDefaultNavigationTimeout(int.Parse(Config.GetSection("Playwright:DefaultNavigationTimeout").Value!));
        page.Context.SetDefaultTimeout(int.Parse(Config.GetSection("Playwright:DefaultTimeout").Value!));

        _objectContainer.RegisterInstanceAs(page);
        _objectContainer.RegisterInstanceAs(page.Context);
        _objectContainer.RegisterInstanceAs(Config);
    }

    [AfterScenario("@web")]
    public async Task Shutdown()
    {
        var isFailed = IsScenarioFailed();
        var testResultsDirectory = Path.GetFullPath(Config.GetSection("Playwright:TestResultsDirectory").Value!);

        TakeScreenshotOnError();
        IList<FileInfo> videoFiles = GetVideoFileNames();

        var page = _objectContainer.Resolve<IPage>();
        var traceRelativePath = Path.Combine("traces", $"{GetTestName()}.zip");
        var traceFullPath = Path.Combine(testResultsDirectory, traceRelativePath);

        await page.Context.Tracing.StopAsync(new() { Path = traceFullPath });
        await page.Context.CloseAsync();
        await page.Context.Browser!.DisposeAsync();
        await page.Context.DisposeAsync();
        
        if (isFailed && File.Exists(traceFullPath))
        {
            TestContext.AddTestAttachment(traceFullPath);
            _reqnrollOutputHelper.AddAttachment(traceFullPath);
        }

        RenameVideoFiles(videoFiles, isFailed);
        
    }

    public string GetTestName()
    {
        var testName = _featureContext.FeatureInfo.Title + "_" + _scenarioContext.ScenarioInfo.Title;

        var testArguments = _scenarioContext.ScenarioInfo.Arguments.Cast<DictionaryEntry>().ToList();
        testArguments.ForEach(entry => testName = testName + "_" + entry.Value);

        return testName;
    }

    public void TakeScreenshotOnError()
    {
        var page = _objectContainer.Resolve<IPage>();

        if (IsScenarioFailed())
        {
            var screenshotsDirectory = Path.Combine(Path.GetFullPath(Config.GetSection("Playwright:TestResultsDirectory").Value!), "screenshots");
            Directory.CreateDirectory(screenshotsDirectory);

            var pages = page.Context.Pages.Count > 0 ? page.Context.Pages.ToList() : [page];
            var screenshotIndex = 0;

            foreach (var currentPage in pages)
            {
                if (currentPage.IsClosed)
                {
                    continue;
                }

                var path = Path.Combine(screenshotsDirectory, $"{GetTestName()}_page_{screenshotIndex}.png");

                currentPage.ScreenshotAsync(new()
                {
                    Path = path,
                    FullPage = true,
                }).Wait();

                // This is to ensure the attachment appears in the Tests tab in Azure DevOps
                TestContext.AddTestAttachment(path);
                _reqnrollOutputHelper.AddAttachment(path);
                screenshotIndex++;
            }

            // Legacy SpecFlow-style context artifact keys are not consumed by Reqnroll HTML reporting.
            // _scenarioContext["PLAYWRIGHT_SCREENSHOTS"] = screenshotPaths;
        }
    }

    private bool IsScenarioFailed()
    {
        return _scenarioContext.TestError != null || _scenarioContext.ScenarioExecutionStatus == ScenarioExecutionStatus.TestError;
    }

    public IList<FileInfo> GetVideoFileNames()
    {
        var page = _objectContainer.Resolve<IPage>();
        IList<FileInfo> fileList = [];

        if (page.Context.Pages.Count > 0)
        {
            for (var i = 0; i < page.Context.Pages.Count; i++)
            {
                if (page.Context.Pages[i].Video != null)
                {
                    fileList.Add(new FileInfo(page.Context.Pages[i].Video.PathAsync().Result));
                }
            }
        }
        else
        {
            if (page.Video != null)
            {
                fileList.Add(new FileInfo(page.Video.PathAsync().Result));
            }
        }

        // Legacy SpecFlow pattern (extra VIDEO_FILENAMES injected via ScenarioContext) is not used in this Reqnroll setup.
        // if (_scenarioContext.ContainsKey("VIDEO_FILENAMES"))
        // {
        //     IList<FileInfo> videofiles = _scenarioContext.Get<IList<FileInfo>>("VIDEO_FILENAMES");
        //
        //     foreach (FileInfo file in videofiles)
        //     {
        //         fileList.Add(file);
        //     }
        // }

        return fileList;
    }

    public IList<string> RenameVideoFiles(IList<FileInfo> videoFiles, bool attachToReport)
    {
        List<string> renamedVideoPaths = [];

        for (var i = 0; i < videoFiles.Count; i++)
        {
            if (videoFiles[i].Exists && videoFiles[i].Directory != null)
            {
                var fileDirectory = videoFiles[i].Directory.FullName;
                var testName = GetTestName();
                var newFileName = $"{testName}_page_{i}.webm";
                var relativePath = Path.Combine("videos", newFileName);
                var fullPath = Path.Combine(fileDirectory, newFileName);

                videoFiles[i].MoveTo(fullPath, true);

                if (attachToReport)
                {
                    TestContext.AddTestAttachment(fullPath);
                    _reqnrollOutputHelper.AddAttachment(fullPath);
                }

                renamedVideoPaths.Add(relativePath);
            }
        }

        return renamedVideoPaths;
    }
}

