using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UdemyMicroservices.Order.Domain.Entities;

namespace UdemyMicroservices.Order.Repository.Configuration;

internal class OrderEntityConfiguration : IEntityTypeConfiguration<Domain.Entities.Order>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Order> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.OrderDate).IsRequired();
        builder.Property(x => x.BuyerId).IsRequired().HasMaxLength(300);
        builder.Property(x => x.TotalPrice).IsRequired().HasColumnType("decimal(18,2)");
        builder.HasMany(x => x.OrderItems).WithOne(y => y.Order).HasForeignKey(x => x.OrderId);
        builder.HasOne(x => x.Address).WithOne(y => y.Order).HasForeignKey<Address>(x => x.OrderId);
    }
}