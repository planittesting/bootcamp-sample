using Microsoft.Playwright;

namespace Bootcamp.Model.Web.Components.UI;
public class Menu
{
    private readonly ILocator _rootElement;    
    protected IPage page;

    public Menu(IPage page)
    {
        this.page = page;        
        _rootElement = page.Locator(".menu");
    }

    public void ClickMenuItem(string menuLinkItem)
    {
        ILocator menuLinkLocator = _rootElement.Locator(".menu-item").GetByRole(AriaRole.Link, new() { Name = menuLinkItem });        

        //These 2 menu items currently open a new page(tab)
        if (menuLinkItem.ToLower().Equals("job planning") || menuLinkItem.ToLower().Equals("pending jobs"))
            page.RunAndWaitForPopupAsync(() => menuLinkLocator.ClickAsync()).Wait();
        else
            menuLinkLocator.ClickAsync().Wait();
    }
    
    public T ClickMenuLink<T>(string menuLinkItem, T target)
    {
        ILocator menuLinkLocator = _rootElement.Locator(".menu-item").GetByRole(AriaRole.Link, new() { Name = menuLinkItem });

        if (menuLinkItem.ToLower().Equals("job planning") || menuLinkItem.ToLower().Equals("pending jobs"))
            page.RunAndWaitForPopupAsync(() => menuLinkLocator.ClickAsync()).Wait();
        else
            menuLinkLocator.ClickAsync().Wait();

        return target;
    }
}
