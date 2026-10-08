using Microsoft.Playwright;

namespace Bootcamp.Drivers;
public class ChromiumFactory : IPageFactory
{
    public IPage GetPage(bool headless, string channel, string videoDir, ViewportSize viewportSize)
    {
        var browser = Playwright.CreateAsync().Result.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = headless,
            Channel = channel,
        }).Result;

        BrowserNewContextOptions browserNewContextOptions = new BrowserNewContextOptions();
        browserNewContextOptions.IgnoreHTTPSErrors = true;
        browserNewContextOptions.ViewportSize = viewportSize;        
        if (!String.IsNullOrEmpty(videoDir))
            browserNewContextOptions.RecordVideoDir = videoDir;

        var context = browser.NewContextAsync(browserNewContextOptions).Result;        
        
        return context.NewPageAsync().Result;
    }
}
