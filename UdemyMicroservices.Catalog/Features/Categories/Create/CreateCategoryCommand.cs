using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Catalog.Features.Categories.Create;

public record CreateCategoryCommand(string Name) : IRequestByServiceResult<CreateCategoryResponse>;