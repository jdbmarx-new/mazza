using MediatR;
using OrderManagement.Application.Common;

namespace OrderManagement.Application.Orders.Create;

public sealed record CreateOrderCommand(Guid CustomerId, IReadOnlyCollection<CreateOrderItem> Items) : IRequest<OrderDto>;
