using AutoMapper;
using UdemyMicroservices.Catalog.Features.Categories.Create;

namespace UdemyMicroservices.Catalog.Features.Categories;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        // CreateCategoryCommand -> Category
        CreateMap<CreateCategoryCommand, Category>();

        CreateMap<CategoryDto, Category>().ReverseMap();
    }
}