using MediatR;
using OrderManagement.Application.Abstractions;
using OrderManagement.Application.Common;
using OrderManagement.Domain.Common;
using OrderManagement.Domain.Orders;

namespace OrderManagement.Application.Orders.Cancel;

public sealed class CancelOrderHandler(IOrderRepository repository) : IRequestHandler<CancelOrderCommand, OrderDto>
{
    public async Task<OrderDto> Handle(CancelOrderCommand r, CancellationToken cancellationToken)
    {
        Order order = await repository.GetByIdAsync(r.Id, true, cancellationToken)
                      ?? throw new NotFoundException($"Order '{r.Id}' was not found.");
        try
        {
            order.Cancel();
        }
        catch (DomainException e)
        {
            throw new ConflictException(e.Message);
        }

        await repository.SaveChangesAsync(cancellationToken);

        return OrderDto.From(order);
    }
}