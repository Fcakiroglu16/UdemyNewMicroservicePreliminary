using AutoMapper;
using MediatR;
using System.Net;
using UdemyMicroservices.Catalog.Repositories;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Catalog.Features.Categories.Create
{
    public class CreateCategoryHandler(IGenericRepository<Category> categoryRepository, IMapper mapper)
        : IRequestHandler<CreateCategoryCommand, ServiceResult<CreateCategoryResponse>>
    {
        public async Task<ServiceResult<CreateCategoryResponse>> Handle(CreateCategoryCommand request,
            CancellationToken cancellationToken)
        {
            // exit category check
            var existingCategory = await categoryRepository.Any(x => x.Name == request.Name);

            if (existingCategory)
            {
                return ServiceResult<CreateCategoryResponse>.Error("Category Name Already Exists",
                    $"The category name '{request.Name}' already exists.", HttpStatusCode.NotFound);
            }


            var category = mapper.Map<Category>(request);

            await categoryRepository.Insert(category);

            return ServiceResult<CreateCategoryResponse>.SuccessAsCreated(new(category.Id), "");
        }
    }
}