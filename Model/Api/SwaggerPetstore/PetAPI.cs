using Bootcamp.Model.Api.SwaggerPetstore;
using Bootcamp.Model.Api.SwaggerPetstore.Data;
using Newtonsoft.Json;

public class PetApi : BaseApi<PetApi>
{
    public PetApi(string baseUrl) : base(baseUrl)
    {
        SetHeaders(new Dictionary<string, string>(){
            {"Content-Type", "application/json; charset=utf-8"}
        });
    }

    public int GetResponseCode() => GetRawResponse().Status;

    public async Task<List<Pet>> FindByStatus(string status)
    {
        List<KeyValuePair<string, object>> dictionary = [new KeyValuePair<string, object>("status", status)];

        SetResourceUrl("/pet/findByStatus");
        SetQueryParameters(dictionary);

        var response = await Get();
        string jsonResponse = await response.TextAsync();
        return JsonConvert.DeserializeObject<List<Pet>>(jsonResponse)!;
    }

    public async Task<Pet> FindById(Int64 petId)
    {
        SetResourceUrl($"/pet/{petId}");

        var response = await Get();
        return JsonConvert.DeserializeObject<Pet>(await response.TextAsync())!;
    }

    public async Task<Pet> CreatePet(string payload)
    {
        SetResourceUrl("/pet");

        var response = await Post(payload);
        return JsonConvert.DeserializeObject<Pet>(await response.TextAsync())!;
    }
}