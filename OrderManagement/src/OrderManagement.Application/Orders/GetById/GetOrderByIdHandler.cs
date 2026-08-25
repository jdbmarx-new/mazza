using MediatR;
using OrderManagement.Application.Abstractions;
using OrderManagement.Application.Common;
using OrderManagement.Domain.Orders;

namespace OrderManagement.Application.Orders.GetById;

public sealed class GetOrderByIdHandler(IOrderRepository repository) : IRequestHandler<GetOrderByIdQuery, OrderDto>
{
    public async Task<OrderDto> Handle(GetOrderByIdQuery r, CancellationToken cancellationToken)
    {
        Order order = await repository.GetByIdAsync(r.Id, false, cancellationToken) ?? throw new NotFoundException($"Order '{r.Id}' was not found.");
        return OrderDto.From(order);
    }
}