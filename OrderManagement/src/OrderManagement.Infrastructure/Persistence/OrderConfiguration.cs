using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Domain.Orders;

namespace OrderManagement.Infrastructure.Persistence;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CustomerId)
               .IsRequired();

        builder.Property(x => x.Status)
               .HasConversion<string>()
               .HasMaxLength(20);

        builder.Property(x => x.CreatedAt)
               .IsRequired();

        builder.Ignore(x => x.TotalAmount);

        builder.HasMany(x => x.Items)
               .WithOne()
               .HasForeignKey(x => x.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Items)
               .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}