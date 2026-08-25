using OrderManagement.Domain.Orders;

namespace OrderManagement.Application.Abstractions;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken);

    Task<Order?> GetByIdAsync(Guid id, bool tracking, CancellationToken cancellationToken);

    Task<(IReadOnlyList<Order> Items, int Total)> GetPageAsync(int page, int pageSize, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}