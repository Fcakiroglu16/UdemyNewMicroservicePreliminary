using UdemyMicroservices.Catalog;
using UdemyMicroservices.Catalog.Features.Categories;
using UdemyMicroservices.Catalog.Features.Courses;
using UdemyMicroservices.Catalog.Repositories;
using UdemyMicroservices.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthenticationExt(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddSwaggerServicesExt();
builder.Services.AddDatabaseServicesExt();
builder.Services.AddCommonServicesExt(typeof(CatalogAssembly));
builder.Services.AddOptionsExt();
builder.Services.AddVersioningExt();

var app = builder.Build();
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
app.AddSwaggerMiddlewareExt();

app.UseAuthentication();
app.UseAuthorization();
var apiVersionSet = app.AddVersionSetExt();
app.AddCategoryEndpointsExt(apiVersionSet);
app.AddCourseEndpointsExt(apiVersionSet);
_ = app.AddSeedDataExt().ContinueWith(task => Console.WriteLine("Seed data has been saved successfully."));
app.Run();