using MassTransit;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using UdemyMicroservices.Catalog.Features.Categories;
using UdemyMicroservices.Catalog.Features.Courses;

namespace UdemyMicroservices.Catalog.Repositories;

public static class SeedData
{
    public static async Task AddSeedDataExt(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var mongoClient = scope.ServiceProvider.GetRequiredService<IMongoClient>();
        var mongoOption = scope.ServiceProvider.GetRequiredService<MongoOption>();
        var dbContext = AppDbContext.Create(mongoClient.GetDatabase(mongoOption.DatabaseName));

        dbContext.Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;

        await SeedDatabaseAsync(dbContext);
    }


    private static async Task SeedDatabaseAsync(AppDbContext context)
    {
        if (!context.Categories.Any())
        {
            var categories = new List<Category>
            {
                new() { Id = NewId.NextGuid(), Name = "Development" },
                new() { Id = NewId.NextGuid(), Name = "Business" },
                new() { Id = NewId.NextGuid(), Name = "IT & Software" },
                new() { Id = NewId.NextGuid(), Name = "Office Productivity" },
                new() { Id = NewId.NextGuid(), Name = "Personal Development" }
            };


            categories.First().Courses =
            [
                new Course
                {
                    Id = NewId.NextGuid(),
                    Name = "C#",
                    Description = "C# Course",
                    Price = 100,
                    UserId = "1",
                    Picture = "1.jpg",
                    CreatedTime = DateTime.Now,
                    Feature = new Feature { Duration = 10, Rating = 4 }
                },

                new Course
                {
                    Id = NewId.NextGuid(),
                    Name = "Java",
                    Description = "Java Course",
                    Price = 200,
                    UserId = "2",
                    Picture = "2.jpg",
                    CreatedTime = DateTime.Now,
                    Feature = new Feature { Duration = 20, Rating = 4 }
                },

                new Course
                {
                    Id = NewId.NextGuid(),
                    Name = "Python",
                    Description = "Python Course",
                    Price = 300,
                    UserId = "3",
                    Picture = "3.jpg",
                    CreatedTime = DateTime.Now,
                    Feature = new Feature { Duration = 30, Rating = 4 }
                }
            ];


            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        //if (!context.Courses.Any())
        //{
        //    var courses = new List<Course>
        //    {
        //        new Course
        //        {
        //            Id = NewId.NextGuid(),
        //            Name = "C#",
        //            Description = "C# Course",
        //            Price = 100,
        //            UserId = "1",
        //            Picture = "1.jpg",
        //            CreatedTime = DateTime.Now,
        //            Feature = new Feature { Duration = 10, Rating = 4 },
        //            CategoryId = context.Categories.First().Id
        //        },
        //        new Course
        //        {
        //            Id = NewId.NextGuid(),
        //            Name = "Java",
        //            Description = "Java Course",
        //            Price = 200,
        //            UserId = "2",
        //            Picture = "2.jpg",
        //            CreatedTime = DateTime.Now,
        //            Feature = new Feature { Duration = 20, Rating = 4 },
        //            CategoryId = context.Categories.First().Id
        //        },
        //        new Course
        //        {
        //            Id = NewId.NextGuid(),
        //            Name = "Python",
        //            Description = "Python Course",
        //            Price = 300,
        //            UserId = "3",
        //            Picture = "3.jpg",
        //            CreatedTime = DateTime.Now,
        //            Feature = new Feature { Duration = 30, Rating = 4 },
        //            CategoryId = context.Categories.First().Id
        //        }
        //    };

        //    await context.Courses.AddRangeAsync(courses);
        //    await context.SaveChangesAsync();
        //}
    }
}