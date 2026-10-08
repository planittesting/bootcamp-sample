namespace Bootcamp.Model.Api.SwaggerPetstore.Data;

public class Category
{
    private int v1;
    private string v2;

    public Category(int v1, string v2)
    {
        this.v1 = v1;
        this.v2 = v2;
    }

    public long Id { get; set; }
    public string Name { get; set; }
}
