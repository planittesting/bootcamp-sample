using Reqnroll.BoDi;
using Bootcamp.Config;

namespace Bootcamp.BDD.Hooks;
[Binding]
public class ApiHooks
{
    private readonly IObjectContainer _objectContainer;    

    public ApiHooks(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;        
    }

    [BeforeScenario("@api")]
    public void SetupAPI()
    {
        PetApi petApi = new(TestConfiguration.GetConfig().GetSection("API_BASEURL").Value!);
        _objectContainer.RegisterInstanceAs(petApi);
    }
}