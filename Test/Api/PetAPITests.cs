using Bootcamp.Model.Api.SwaggerPetstore;
using Bootcamp.Model.Api.SwaggerPetstore.Data;
using Bootcamp.Support.Utils;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Bootcamp.Test.SwaggerPetStoreApiTests;

public class PetApiTests : BaseApiTests
{
    [Test]
    public async Task CreatePet()
    {
        var petApi = new PetApi(baseUrl);
        var expectedPet = new Pet(
            Utils.GeneratePositiveRandomInt(),
            new Category(0, "string"),
            Utils.GenerateRandomString(8),
            ["string"],
            [new Tag(0, "string")],
            "available");

        var createdPet = await petApi.CreatePet(JsonConvert.SerializeObject(expectedPet, Formatting.Indented));
        Assert.That(petApi.GetResponseCode(), Is.EqualTo(200));

        var fetchedPet = await petApi.FindById(createdPet.Id);
        Assert.That(fetchedPet.Id, Is.EqualTo(createdPet.Id));
    }

    [Test]
    public async Task FindPetById()
    {
        var petApi = new PetApi(baseUrl);
        var pets = await petApi.FindByStatus("available");

        Assert.That(pets, Is.Not.Empty);

        var expectedId = pets.First().Id;
        var pet = await petApi.FindById(expectedId);

        Assert.That(petApi.GetResponseCode(), Is.EqualTo(200));
        Assert.That(pet.Id, Is.EqualTo(expectedId));
    }

    [TestCase("available")]
    [TestCase("sold")]
    [TestCase("pending")]
    public async Task FindPetsByStatus(string status)
    {
        var petApi = new PetApi(baseUrl);
        var pets = await petApi.FindByStatus(status);

        Assert.That(petApi.GetResponseCode(), Is.EqualTo(200));

        foreach (var pet in pets)
        {
            Assert.That(pet.Status, Is.EqualTo(status));
        }
    }
}
