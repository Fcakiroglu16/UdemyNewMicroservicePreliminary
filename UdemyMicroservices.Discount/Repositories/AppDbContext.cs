using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace UdemyMicroservices.Discount.Repositories;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Discount> Discounts { get; set; } = default!;

    public static AppDbContext Create(IMongoDatabase database)
    {
        return new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
            .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
            .Options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // discount entity configuration
        modelBuilder.Entity<Discount>().ToCollection("discounts");
        modelBuilder.Entity<Discount>().HasKey(x => x.Id);
        modelBuilder.Entity<Discount>().Property(x => x.Id).ValueGeneratedNever();
        modelBuilder.Entity<Discount>().Property(x => x.UserId).HasElementName("user_id").HasMaxLength(50);
        modelBuilder.Entity<Discount>().Property(x => x.Rate).HasElementName("rate");
        modelBuilder.Entity<Discount>().Property(x => x.Code).HasElementName("code").HasMaxLength(50);
        modelBuilder.Entity<Discount>().Property(x => x.Created).HasElementName("created");
        modelBuilder.Entity<Discount>().Property(x => x.Expired).HasElementName("expired");


        base.OnModelCreating(modelBuilder);
    }
}