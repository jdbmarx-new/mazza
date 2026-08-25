using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OrderManagement.Domain.Orders;

#nullable disable

namespace OrderManagement.Infrastructure.Persistence.Migrations;

[DbContext(typeof(AppDbContext))]
internal partial class AppDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("ProductVersion", "10.0.0");

        modelBuilder.Entity<Order>(e =>
        {
            e.Property(p => p.Id).HasColumnType("TEXT");
            e.Property(p => p.CreatedAt).HasColumnType("TEXT");
            e.Property(p => p.CustomerId).HasColumnType("TEXT");
            e.Property(p => p.Status).IsRequired().HasMaxLength(20).HasColumnType("TEXT");
            e.HasKey(p => p.Id);
            e.ToTable("Orders");
        });

        modelBuilder.Entity<OrderItem>(e =>
        {
            e.Property(p => p.Id).HasColumnType("TEXT");
            e.Property(p => p.OrderId).HasColumnType("TEXT");
            e.Property(p => p.ProductName).IsRequired().HasMaxLength(200).HasColumnType("TEXT");
            e.Property(p => p.Quantity).HasColumnType("INTEGER");
            e.Property(p => p.UnitPrice).HasPrecision(18, 2).HasColumnType("TEXT");
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.OrderId);
            e.ToTable("OrderItems");
        });

        modelBuilder.Entity<OrderItem>(e => { e.HasOne("OrderManagement.Domain.Orders.Order", null).WithMany("Items").HasForeignKey("OrderId").OnDelete(DeleteBehavior.Cascade).IsRequired(); });
    }
}