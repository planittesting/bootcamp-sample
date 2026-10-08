using Bootcamp.Config;
using Bootcamp.Model.Web.Pages;
using Bootcamp.Test.DataProviders;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Text.RegularExpressions;
using static Microsoft.Playwright.Assertions;

namespace Bootcamp.Test.JupiterPrimeUiTests;

public class SubscriptionsTestSuite : BaseTestSuite
{
    private static readonly IConfiguration Config = TestConfiguration.GetConfig();

    [Test]
    public async Task NavigateToSubscriptionPageViaSubscribeLink()
    {
        await page.GotoAsync(Config.GetSection("BASEURL").Value!);
        await Open<SubscribePage>().ClickSubscribeLink();

        await Expect(page).ToHaveURLAsync(new Regex("/subscribe$"));
        await Expect(page).ToHaveTitleAsync("AEMO | Subscribe");
    }

    [Test]
    public async Task SubmitEmptySubscriptionFormShowsAllRequiredFieldErrors()
    {
        await NavigateToSubscribePage();
        await Open<SubscribePage>().ClickSubmitButton();

        var subscribePage = Open<SubscribePage>();
        await subscribePage.VerifyRequiredFieldErrorIsVisible("First Name");
        await subscribePage.VerifyRequiredFieldErrorIsVisible("Last Name");
        await subscribePage.VerifyRequiredFieldErrorIsVisible("Email");
    }

    [TestCase("First Name")]
    [TestCase("Last Name")]
    [TestCase("Email")]
    public async Task SubmitEmptySubscriptionFormShowsRequiredFieldError(string fieldName)
    {
        await NavigateToSubscribePage();
        await Open<SubscribePage>().ClickSubmitButton();

        await Open<SubscribePage>().VerifyRequiredFieldErrorIsVisible(fieldName);
    }

    [TestCaseSource(typeof(SubscriptionFieldDataProvider))]
    public async Task SubmitEmptySubscriptionFormShowsRequiredFieldErrorWithDataProvider(string fieldName)
    {
        await NavigateToSubscribePage();
        await Open<SubscribePage>().ClickSubmitButton();

        await Open<SubscribePage>().VerifyRequiredFieldErrorIsVisible(fieldName);
    }

    private async Task NavigateToSubscribePage()
    {
        await page.GotoAsync(Config.GetSection("BASEURL").Value!);
        await Open<SubscribePage>().ClickSubscribeLink();
    }
}
