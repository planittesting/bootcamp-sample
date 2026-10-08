using System.Reflection;
using Microsoft.Playwright;

namespace Bootcamp.Drivers;
public interface IPageFactory
{
    public IPage GetPage(bool headless, string channel, string videoDir, ViewportSize viewportSize);

    public static IPage BuildPage(string browser, string channel, bool headless = true, string videoDir = null, ViewportSize viewportSize = null)
    {
        var factoryType = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type => type.GetInterface(typeof(IPageFactory).ToString()) != null)
            .FirstOrDefault(type => type.Name.Contains(browser, StringComparison.InvariantCultureIgnoreCase))
            ?? throw new NotSupportedException($"Browser {browser} not suported");

        var pageFactory = Activator.CreateInstance(factoryType) as IPageFactory;
        return pageFactory!.GetPage(headless, channel, videoDir, viewportSize);
    }
}
