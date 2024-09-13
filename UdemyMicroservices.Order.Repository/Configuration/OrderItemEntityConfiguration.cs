using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UdemyMicroservices.Order.Domain.Entities;

namespace UdemyMicroservices.Order.Repository.Configuration;

internal class OrderItemEntityConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ProductId).IsRequired().HasMaxLength(100);
        builder.Property(x => x.ProductName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");
    }
}