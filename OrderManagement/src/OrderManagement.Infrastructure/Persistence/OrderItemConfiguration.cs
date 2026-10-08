using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Domain.Orders;

namespace OrderManagement.Infrastructure.Persistence;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProductName)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(x => x.UnitPrice)
               .HasPrecision(18, 2);

        builder.Ignore(x => x.Total);
    }
}