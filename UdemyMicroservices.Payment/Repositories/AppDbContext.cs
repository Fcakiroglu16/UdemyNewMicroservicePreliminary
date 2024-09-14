using Microsoft.EntityFrameworkCore;

namespace UdemyMicroservices.Payment.Repositories
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)

    {
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //payment configuration
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(x => x.Id).ValueGeneratedNever();
                entity.Property(e => e.UserId).IsRequired().HasMaxLength(50);
                entity.Property(e => e.OrderCode).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Amount).IsRequired();
                entity.Property(e => e.PaymentDate).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.Error).HasMaxLength(200);
            });


            base.OnModelCreating(modelBuilder);
        }
    }
}