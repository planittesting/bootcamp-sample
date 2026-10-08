using Microsoft.Playwright;

namespace Bootcamp.Model.Api.SwaggerPetstore;
public abstract class BaseApi<T> where T : BaseApi<T>
{
    protected string baseUrl;
    protected IAPIResponse? rawResponse;
    private string resourceUrl;
    private Dictionary<string, string> headers;
    private IEnumerable<KeyValuePair<string, object>> queryParameters;
    private readonly Task<IAPIRequestContext> clientTask;


    public BaseApi(string baseUrl)
    {
        this.baseUrl = baseUrl;
        headers = new Dictionary<string, string>();
        queryParameters = new List<KeyValuePair<string, object>>();
        clientTask = CreateClientAsync(baseUrl);
    }

    private static async Task<IAPIRequestContext> CreateClientAsync(string baseUrl)
    {
        var playwright = await Playwright.CreateAsync();
        return await playwright.APIRequest.NewContextAsync(new() { BaseURL = baseUrl });
    }

    public async Task<IAPIResponse> Get()
    {
        APIRequestContextOptions options = new()
        {
            Params = queryParameters,
            Method = "get"
        };

        return await SendRequest(options);
    }

    public async Task<IAPIResponse> Post(string payload)
    {
        APIRequestContextOptions options = new()
        {
            Params = queryParameters,
            Method = "post",
            DataString = payload
        };

        return await SendRequest(options);
    }

    private async Task<IAPIResponse> SendRequest(APIRequestContextOptions options)
    {
        options.Headers = headers;
        var client = await clientTask;
        rawResponse = await client.FetchAsync(resourceUrl, options);
        return rawResponse;
    }

    public IAPIResponse GetRawResponse() => rawResponse ?? throw new InvalidOperationException("No API response is available yet.");

    public void SetHeaders(Dictionary<string, string> headers) => this.headers = headers;

    public void SetQueryParameters(IEnumerable<KeyValuePair<string, object>> queryParameters) => this.queryParameters = queryParameters;

    protected void SetResourceUrl(string resourceUrl)
    {
        // Remove leading slash as not needed for RestSharp implementation
        if (resourceUrl.StartsWith('/') || resourceUrl.StartsWith('\\'))
        {
            resourceUrl = resourceUrl[1..];
        }
        this.resourceUrl = resourceUrl;
    }
}
