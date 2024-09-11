using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using UdemyMicroservices.Catalog.Features.Categories;
using UdemyMicroservices.Catalog.Features.Courses;

namespace UdemyMicroservices.Catalog.Repositories
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Category> Categories { get; init; }
        public DbSet<Course> Courses { get; init; }

        public static AppDbContext Create(IMongoDatabase database) =>
            new(new DbContextOptionsBuilder<AppDbContext>()
                .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
                .Options);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Course entity configuration
            modelBuilder.Entity<Course>().ToCollection("courses");
            modelBuilder.Entity<Course>().HasKey(x => x.Id);
            modelBuilder.Entity<Course>().Property(x => x.Id).ValueGeneratedNever();
            modelBuilder.Entity<Course>().Property(x => x.Name).HasElementName("name").HasMaxLength(100);
            modelBuilder.Entity<Course>().Property(x => x.Description).HasElementName("description").HasMaxLength(500);


            modelBuilder.Entity<Course>().Property(x => x.Price).HasElementName("price");
            modelBuilder.Entity<Course>().Property(x => x.Picture).HasElementName("picture").HasMaxLength(200);
            modelBuilder.Entity<Course>().Property(x => x.UserId).HasElementName("user_id").HasMaxLength(50);
            modelBuilder.Entity<Course>().Property(x => x.CreatedTime).HasElementName("created_time");
            modelBuilder.Entity<Course>().Property(x => x.CategoryId).HasElementName("category_id");
            //modelBuilder.Entity<Course>().Property(x => x.Feature).HasElementName("feature");

            modelBuilder.Entity<Course>().Ignore(x => x.Category);
            // Category entity configuration
            modelBuilder.Entity<Category>().ToCollection("categories");
            modelBuilder.Entity<Category>().HasKey(x => x.Id);
            modelBuilder.Entity<Category>().Property(x => x.Id).ValueGeneratedNever();
            modelBuilder.Entity<Category>().Property(x => x.Name).HasElementName("name").HasMaxLength(100);
            modelBuilder.Entity<Category>().Ignore(x => x.Courses);


            // Configure Feature as an owned entity
            modelBuilder.Entity<Course>().OwnsOne(p => p.Feature, feature =>
            {
                feature.HasElementName("feature");
                feature.Property(f => f.Duration).HasElementName("duration");
                feature.Property(f => f.Rating).HasElementName("rating");
                // Add other properties of Feature if needed
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}