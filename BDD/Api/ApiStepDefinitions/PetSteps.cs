using Utils = Bootcamp.Support.Utils.Utils;
using Reqnroll.BoDi;
using Bootcamp.Model.Api.SwaggerPetstore.Data;
using Newtonsoft.Json;
using NUnit.Framework;
using static Microsoft.Playwright.Assertions;

namespace Bootcamp.BDD.Api.ApiStepDefinitions;
[Binding]
public class PetSteps : BaseApiSteps
{
    private ScenarioContext scenarioContext;

    public PetSteps(ScenarioContext scenarioContext, IObjectContainer objectContainer) : base(objectContainer)
    {
        this.scenarioContext = scenarioContext;
    }

    [Given(@"I am authenticated and authorised")]
    public void GivenIAmAuthenticatedAndAuthorised()
    {        
        // Auth code goes here
    }

    [Given(@"a valid pet Id")]
    public async Task GivenAValidPetId()
    {
        List<Pet> pets = await petApi.FindByStatus("available");

        Console.WriteLine($"Pet Id is: {pets.First().Id}.");

        scenarioContext["PET_ID"] = pets.First().Id;
    }

    [When(@"I GET a list of '([^']*)' pets")]
    public async Task WhenIGetAListOfPets(string status)
    {
        List<Pet> pets = await petApi.FindByStatus(status);

        scenarioContext["PET_API"] = petApi;
        scenarioContext["PETS_LIST"] = pets;
    }

    [When(@"I GET a pet by id")]
    public async Task WhenIGETAPetById()
    {
        long petId = scenarioContext.Get<long>("PET_ID");

        Pet pet = await petApi.FindById(petId);

        scenarioContext["PET"] = pet;
        scenarioContext["PET_API"] = petApi;
    }

    [Then(@"the pet with the matching id should be returned")]
    public async Task ThenThePetWithByTheMatchingIdShouldBeReturned()
    {
        Pet pet = scenarioContext.Get<Pet>("PET");
        long id = scenarioContext.Get<long>("PET_ID");
        PetApi petApi = scenarioContext.Get<PetApi>("PET_API");

        await Expect(petApi.GetRawResponse()).ToBeOKAsync();
        Assert.That(pet.Id, Is.EqualTo(id));
    }

    [When(@"I POST a new pet")]
    public async Task WhenIPOSTANewPet()
    {
        long id = Utils.GeneratePositiveRandomInt();
        string name = Utils.GenerateRandomString(8);

        Pet petRecord = new Pet(
        Utils.GeneratePositiveRandomInt(),
            new Category(0, "string"),
            $"{name}",
            ["string"],
            [new Tag(0, "string")],
            "available");

        Pet pet = await petApi.CreatePet(JsonConvert.SerializeObject(petRecord, Formatting.Indented));

        scenarioContext["PET"] = pet;
        scenarioContext["PET_API"] = petApi;
    }

    [Then(@"the pet should be created")]
    public async Task ThenThePetShouldBeCreated()
    {
        PetApi petApi = scenarioContext.Get<PetApi>("PET_API");
        Pet pet = scenarioContext.Get<Pet>("PET");
        long newPetId = pet.Id;

        Console.WriteLine($"INFO: Created pet with id {newPetId}");

        await Expect(petApi.GetRawResponse()).ToBeOKAsync();

        pet = await petApi.FindById(pet.Id);        
        Assert.That(pet.Id, Is.EqualTo(newPetId));
    }

    [Then(@"the status of each pet returned should be (.*)$")]
    public void ThenTheStatusReturnedShouldBeAvailable(string status)
    {
        List<Pet> pets = scenarioContext.Get<List<Pet>>("PETS_LIST");

        Console.WriteLine($"Number of {status} pets returned is: {pets.Count}.");

        foreach (Pet pet in pets)
        {
            Assert.That(pet.Status, Is.EqualTo(status));
        }
    }
}

