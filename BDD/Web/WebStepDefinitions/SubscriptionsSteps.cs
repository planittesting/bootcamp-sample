using Bootcamp.Model.Web.Pages;
using NUnit.Framework;
using Reqnroll.BoDi;
using System.Text.RegularExpressions;
using static Microsoft.Playwright.Assertions;

namespace Bootcamp.BDD.Web.WebStepDefinitions;
[Binding]
public class SubscriptionsSteps : BaseSteps
{
    private readonly ScenarioContext _scenarioContext;

    public SubscriptionsSteps(IObjectContainer objectContainer, ScenarioContext scenarioContext) : base(objectContainer)
    {
        _scenarioContext = scenarioContext;
    }

    [Given("a visitor is on the AEMO home page")]
    public async Task VisitorOnTheAEMOHomePage()
    {
        await page.GotoAsync(Config.GetSection("BASEURL").Value!);
    }

    [When("they choose to subscribe")]
    public async Task ClickSubscribe()
    {        
        await Open<SubscribePage>().ClickSubscribeLink();
    }

    [Then("the subscription page is displayed")]
    public async Task SubscriptionPageIsDisplayed()
    {
        await Expect(page).ToHaveURLAsync(new Regex("/subscribe$"));
        await Expect(page).ToHaveTitleAsync("AEMO | Subscribe");
    }

    [Given("a visitor is on the Subscribe page")]
    public async Task VisitorOnTheSubscribePage()
    {
        await page.GotoAsync(Config.GetSection("BASEURL").Value!);
        await Open<SubscribePage>().ClickSubscribeLink();
    }

    [When("they submit an empty subscription form")]
    public async Task SubmitSubscription()
    {        
        await Open<SubscribePage>().ClickSubmitButton();
    }

    [Then("an error is shown for each required field:")]
    public async Task VerifyRequiredFields(DataTable dataTable)
    {
        await Open<SubscribePage>().VerifyRequiredFieldErrorsAreVisible(dataTable);
    }

    [Then("an error is shown for required field {string}")]
    public async Task ThenAnErrorIsShownForRequiredField(string fieldName)
    {
        await Open<SubscribePage>().VerifyRequiredFieldErrorIsVisible(fieldName);
    }
}