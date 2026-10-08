using Reqnroll.BoDi;
using Bootcamp.Model.Api.SwaggerPetstore;

public abstract class BaseApiSteps
{
    protected IObjectContainer objectContainer;
    protected PetApi petApi;

    protected BaseApiSteps(IObjectContainer objectContainer)
    {
        petApi = objectContainer.Resolve<PetApi>();
        this.objectContainer = objectContainer;
    }
}
