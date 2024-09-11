using UdemyMicroservices.Catalog;
using UdemyMicroservices.Catalog.Features.Categories;
using UdemyMicroservices.Catalog.Features.Courses;
using UdemyMicroservices.Catalog.Repositories;
using UdemyMicroservices.Shared;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSwaggerServicesExt();
builder.Services.AddDatabaseServicesExt();
builder.Services.AddCommonServicesExt(typeof(CatalogAssembly));
builder.Services.AddOptionsExt();
builder.Services.AddVersioningExt();

var app = builder.Build();


// Configure the HTTP request pipeline.
app.AddSwaggerMiddlewareExt();

var apiVersionSet = app.AddVersionSetExt();
app.AddCategoryEndpointsExt(apiVersionSet);
app.AddCourseEndpointsExt(apiVersionSet);
_ = app.AddSeedDataExt().ContinueWith(task => Console.WriteLine("Seed data has been saved successfully."));
app.Run();