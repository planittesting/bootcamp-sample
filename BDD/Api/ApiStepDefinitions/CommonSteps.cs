using Reqnroll.BoDi;
using NUnit.Framework;
using static Microsoft.Playwright.Assertions;

namespace OriginBulkTransportTests.BDD.Api.ApiStepDefinitions;
[Binding]
public class CommonSteps : BaseApiSteps
{
    private ScenarioContext scenarioContext;

    public CommonSteps(ScenarioContext scenarioContext, IObjectContainer objectContainer) : base(objectContainer)
    {
        this.scenarioContext = scenarioContext;
    }

    [Then(@"the response code should be (.*)")]
    public async Task ThenTheResponseCodeShouldBe(int responseCode)
    {
        PetApi petApi = scenarioContext.Get<PetApi>("PET_API");
        var response = petApi.GetRawResponse();

        Console.WriteLine("API_RESPONSE" + await response.TextAsync());

        if (responseCode == 200)
        {
            await Expect(response).ToBeOKAsync();
            return;
        }

        Assert.That(response.Status, Is.EqualTo(responseCode));
    }
}
