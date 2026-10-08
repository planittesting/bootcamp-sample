using Microsoft.Playwright;
using NUnit.Framework;
using static Microsoft.Playwright.Assertions;

namespace Bootcamp.Model.Web.Pages
{
    public class SubscribePage : BasePage<SubscribePage>
    { 
        private readonly ILocator submitButton;
        private readonly ILocator firstNameErrorMessage;
        private readonly ILocator lastNameErrorMessage;
        private readonly ILocator emailErrorMessage;        

        public SubscribePage(IPage page) : base(page)
        {            
            submitButton = page.GetByRole(AriaRole.Button, new() { Name = "Submit" });
            firstNameErrorMessage = page.Locator("input[name='firstName'] + p");
            lastNameErrorMessage = page.Locator("input[name='lastName'] + p");
            emailErrorMessage = page.Locator("input[name='email'] + p");            
        }

        public async Task ClickSubmitButton() {
            // Wait for recaptcha to load before clicking the submit button
            var recaptcha = page.FrameLocator("iframe[title='reCAPTCHA']");
            await recaptcha.Locator("#rc-anchor-container").WaitForAsync(new() { State = WaitForSelectorState.Attached });            
            
            await submitButton.ClickAsync(); 
        }
        
        private ILocator GetErrorLocator(string fieldName)
        {
            switch (fieldName.ToLowerInvariant())
            {
                case "first name":
                    return firstNameErrorMessage;
                case "last name":
                    return lastNameErrorMessage;
                case "email":
                    return emailErrorMessage;                
                default:
                    throw new ArgumentException($"No error locator defined for field '{fieldName}'");
            }
        }

        public async Task VerifyRequiredFieldErrorIsVisible(string fieldName)
        {
            var errorLocator = GetErrorLocator(fieldName);
            await Expect(errorLocator, $"Expected an error message for field '{fieldName}' to be visible").ToBeVisibleAsync();
        }

        public async Task VerifyRequiredFieldErrorsAreVisible(DataTable dataTable)
        {
            var failures = new List<string>();

            foreach (var row in dataTable.Rows)
            {
                var fieldName = row["Field Name"];
                var errorLocator = GetErrorLocator(fieldName);                              

                try
                {
                    await Expect(errorLocator, $"Expected an error message for field '{fieldName}' to be visible").ToBeVisibleAsync();                    
                }
                catch (PlaywrightException)
                {
                    failures.Add(fieldName);
                }
            }

            Assert.That(failures, Is.Empty, $"No error message visible for: {string.Join(", ", failures)}");
        }
    }
}