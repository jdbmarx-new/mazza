using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Abstractions;
using OrderManagement.Domain.Orders;

namespace OrderManagement.Infrastructure.Persistence;

public sealed class OrderRepository(AppDbContext db) : IOrderRepository
{
    public Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        return db.Orders.AddAsync(order, cancellationToken)
                        .AsTask();
    }

    public Task<Order?> GetByIdAsync(Guid id, bool tracking, CancellationToken cancellationToken)
    {
        IQueryable<Order> q = db.Orders.Include(x => x.Items).AsQueryable();
        if (!tracking)
        {
            q = q.AsNoTracking();
        }
        return q.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<Order> Items, int Total)> GetPageAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        IOrderedQueryable<Order> q = db.Orders.AsNoTracking().Include(x => x.Items).OrderByDescending(x => x.CreatedAt);

        int total = await q.CountAsync(cancellationToken);

        List<Order> items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return (items, total);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return db.SaveChangesAsync(cancellationToken);
    }
}