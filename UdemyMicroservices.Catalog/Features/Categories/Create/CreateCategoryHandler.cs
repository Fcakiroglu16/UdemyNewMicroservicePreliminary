using AutoMapper;
using MediatR;
using System.Net;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using UdemyMicroservices.Catalog.Repositories;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Catalog.Features.Categories.Create
{
    public class CreateCategoryHandler(AppDbContext context, IMapper mapper)
        : IRequestHandler<CreateCategoryCommand, ServiceResult<CreateCategoryResponse>>
    {
        public async Task<ServiceResult<CreateCategoryResponse>> Handle(CreateCategoryCommand request,
            CancellationToken cancellationToken)
        {
            // exit category check
            var existingCategory =
                await context.Categories.AnyAsync(x => x.Name == request.Name, cancellationToken: cancellationToken);

            if (existingCategory)
            {
                return ServiceResult<CreateCategoryResponse>.Error("Category Name Already Exists",
                    $"The category name '{request.Name}' already exists.", HttpStatusCode.NotFound);
            }


            var category = mapper.Map<Category>(request);

            category.Id = NewId.NextGuid();
            await context.AddAsync(category, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);


            return ServiceResult<CreateCategoryResponse>.SuccessAsCreated(new(category.Id.ToString()), "");
        }
    }
}