using MediatR;
using OrderManagement.Application.Abstractions;
using OrderManagement.Application.Common;
using OrderManagement.Domain.Orders;

namespace OrderManagement.Application.Orders.Create;

public sealed class CreateOrderHandler(IOrderRepository repository, IClock clock) : IRequestHandler<CreateOrderCommand, OrderDto>

{
    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = Order.Create(request.CustomerId, request.Items.Select(i => (i.ProductName, i.Quantity, i.UnitPrice)), clock.UtcNow);
        await repository.AddAsync(order, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return OrderDto.From(order);
    }
}