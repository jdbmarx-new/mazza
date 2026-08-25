namespace OrderManagement.Api;

public sealed record CreateOrderItemRequest(string ProductName, int Quantity, decimal UnitPrice);