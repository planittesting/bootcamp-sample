using Microsoft.Playwright;

namespace Bootcamp.Model.Web.Components.Dialogs
{
    public abstract class BaseDialog<T>
    {
        protected IPage page;
        protected T parent;
        protected ILocator rootElement;

        public BaseDialog(IPage page, string name, T parent)
        {
            this.page = page;
            this.parent = parent;
            rootElement = page.GetByRole(AriaRole.Dialog, new() { Name = name });
        }

        public T ClickOkButton()
        {
            rootElement.GetByRole(AriaRole.Button, new() { Name = "Ok" }).ClickAsync().Wait();
            return parent;
        }

        public T ClickCancelButton()
        {
            rootElement.GetByRole(AriaRole.Button, new() { Name = "Cancel" }).ClickAsync().Wait();
            return parent;
        }
    }
}
