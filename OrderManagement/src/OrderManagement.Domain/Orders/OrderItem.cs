using OrderManagement.Domain.Common;

namespace OrderManagement.Domain.Orders;

public sealed class OrderItem
{
    private OrderItem()
    { ProductName = string.Empty; }

    internal OrderItem(Guid orderId, string productName, int quantity, decimal unitPrice)
    {
        if (orderId == Guid.Empty)
        {
            throw new DomainException("OrderId is required.");
        }

        if (string.IsNullOrWhiteSpace(productName))
        {
            throw new DomainException("Product name is required.");
        }

        if (quantity <= 0)
        {
            throw new DomainException("Quantity must be greater than zero.");
        }

        if (unitPrice <= 0)
        {
            throw new DomainException("Unit price must be greater than zero.");
        }

        Id = Guid.NewGuid();
        OrderId = orderId;
        ProductName = productName.Trim();
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Total => Quantity * UnitPrice;
}