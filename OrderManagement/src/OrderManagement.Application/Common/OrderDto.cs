using OrderManagement.Domain.Orders;

namespace OrderManagement.Application.Common;

public sealed record OrderDto(Guid Id, Guid CustomerId, OrderStatus Status, DateTime CreatedAt, decimal TotalAmount, IReadOnlyCollection<OrderItemDto> Items)
{
    public static OrderDto From(Order o)
    {
        return new(o.Id, o.CustomerId, o.Status, o.CreatedAt, o.TotalAmount, [.. o.Items.Select(i => new OrderItemDto(i.Id, i.ProductName, i.Quantity, i.UnitPrice, i.Total))]);
    }
}
