using Bootcamp.Model.Web.Pages;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using Microsoft.Testing.Platform.Configurations;
using Reqnroll.BoDi;

namespace Bootcamp.BDD.Web.WebStepDefinitions;
public abstract class BaseSteps
{
    protected readonly IPage page;
    protected IObjectContainer objectContainer;
    protected static Microsoft.Extensions.Configuration.IConfiguration Config;

    protected BaseSteps(IObjectContainer objectContainer)
    {
        page = objectContainer.Resolve<IPage>();
        Config = objectContainer.Resolve<Microsoft.Extensions.Configuration.IConfiguration>();
        this.objectContainer = objectContainer;
    }

    protected T Open<T>(IPage page = null) where T : BasePage<T>
    {
        if (page == null)
            return ((BasePage<T>)typeof(T).GetConstructor(new[] { typeof(IPage) }).Invoke(new object[] { this.page })).Init();
        else
            return ((BasePage<T>)typeof(T).GetConstructor(new[] { typeof(IPage) }).Invoke(new object[] { page })).Init();
    }
}
