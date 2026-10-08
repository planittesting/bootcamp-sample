using Microsoft.Playwright;

namespace Bootcamp.Model.Web.Pages;
public abstract class BasePage<T> where T : BasePage<T>
{
    protected IPage page;    

    protected BasePage(IPage page)
    {
        this.page = page;
        
    }

    public virtual T? Init()
    {
        return this as T;
    }

    public async Task ClickSubscribeLink() => await page.GetByRole(AriaRole.Link, new() { Name = "Subscribe" }).ClickAsync();
}