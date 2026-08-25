namespace OrderManagement.Application.Orders.Create;

public sealed record CreateOrderItem(string ProductName, int Quantity, decimal UnitPrice);
