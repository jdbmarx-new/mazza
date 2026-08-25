using OrderManagement.Application.Abstractions;
using OrderManagement.Domain.Orders;

namespace OrderManagement.UnitTests;

internal sealed class FakeOrderRepository : IOrderRepository
{
    public readonly List<Order> Orders = [];

    public Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        Orders.Add(order);
        return Task.CompletedTask;
    }

    public Task<Order?> GetByIdAsync(Guid id, bool tracking, CancellationToken cancellationToken)
    {
        return Task.FromResult(Orders.SingleOrDefault(x => x.Id == id));
    }

    public Task<(IReadOnlyList<Order> Items, int Total)> GetPageAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        return Task.FromResult(((IReadOnlyList<Order>)[.. Orders.Skip((page - 1) * pageSize).Take(pageSize)],
                                Orders.Count));
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}