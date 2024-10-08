using Microsoft.Extensions.FileProviders;
using UdemyMicroservices.Catalog;
using UdemyMicroservices.Files.Features.File;
using UdemyMicroservices.Shared;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthenticationExt(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddSwaggerServicesExt();
builder.Services.AddCommonServicesExt(typeof(FileAssembly));
builder.Services.AddVersioningExt();

//add file service
builder.Services.AddSingleton<IFileProvider>(
    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));


var app = builder.Build();

app.UseExceptionHandler();
app.AddSwaggerMiddlewareExt();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
var apiVersionSet = app.AddVersionSetExt();
app.AddFileEndpointsExt(apiVersionSet);


app.Run();