using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UdemyMicroservices.Catalog;
using UdemyMicroservices.Catalog.Features.Categories;
using UdemyMicroservices.Catalog.Features.Courses;
using UdemyMicroservices.Catalog.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var mongoOption = sp.GetRequiredService<MongoOption>();
    return new MongoClient(mongoOption.ConnectionString);
});

builder.Services.AddScoped(sp =>
{
    var mongoClient = sp.GetRequiredService<IMongoClient>();
    var mongoOption = sp.GetRequiredService<MongoOption>();
    return AppDbContext.Create(mongoClient.GetDatabase(mongoOption.DatabaseName));
});


//generic
//automapper
builder.Services.AddAutoMapper(typeof(CatalogAssembly));
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CatalogAssembly>();
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<CatalogAssembly>());

/*
AddOptions<MongoOption>(): MongoOption yapılandırma sınıfını dependency injection'a ekler.
BindConfiguration(nameof(MongoOption)): MongoOption sınıfını appsettings.json gibi yapılandırma kaynaklarına bağlar.
ValidateDataAnnotations(): MongoOption sınıfındaki DataAnnotation doğrulamalarını uygular.
ValidateOnStart(): Uygulama başlatılırken yapılandırma verilerini doğrular.
 */
builder.Services
    .AddOptions<MongoOption>()
    .BindConfiguration(nameof(MongoOption))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<MongoOption>>().Value);


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddCategoryEndpointsExt();
app.AddCourseEndpointsExt();
await app.AddSeedDataExt();
app.Run();