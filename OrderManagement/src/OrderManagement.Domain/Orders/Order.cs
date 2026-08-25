using OrderManagement.Domain.Common;

namespace OrderManagement.Domain.Orders;

public sealed class Order
{
    private readonly List<OrderItem> _items = [];

    private Order()
    { }

    private Order(Guid customerId, DateTime createdAt)
    {
        if (customerId == Guid.Empty)
        {
            throw new DomainException("CustomerId is required.");
        }

        Id = Guid.NewGuid();
        CustomerId = customerId;
        Status = OrderStatus.Pending;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public decimal TotalAmount => _items.Sum(x => x.Total);

    public static Order Create(Guid customerId, IEnumerable<(string ProductName, int Quantity, decimal UnitPrice)> items, DateTime now)
    {
        var order = new Order(customerId, now);
        foreach ((string ProductName, int Quantity, decimal UnitPrice) item in items)
        {
            order.AddItem(item.ProductName, item.Quantity, item.UnitPrice);
        }

        return order._items.Count == 0
            ? throw new DomainException("An order must contain at least one item.")
            : order;
    }

    private void AddItem(string name, int quantity, decimal price)
    {
        _items.Add(new OrderItem(Id, name, quantity, price));
    }

    public void Cancel()
    {
        if (Status != OrderStatus.Pending)
        {
            throw new DomainException("Only pending orders can be cancelled.");
        }

        Status = OrderStatus.Cancelled;
    }
}