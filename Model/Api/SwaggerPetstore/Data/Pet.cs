namespace Bootcamp.Model.Api.SwaggerPetstore.Data;

public record Pet(long Id, Category Category, string Name, List<string> PhotoUrls, List<Tag> Tags, string Status);
