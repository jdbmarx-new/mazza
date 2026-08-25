namespace OrderManagement.Application.Common;

public sealed record OrderItemDto(Guid Id, string ProductName, int Quantity, decimal UnitPrice, decimal Total);
