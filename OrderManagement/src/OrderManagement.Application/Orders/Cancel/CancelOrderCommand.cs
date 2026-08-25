using MediatR;
using OrderManagement.Application.Common;

namespace OrderManagement.Application.Orders.Cancel;

public sealed record CancelOrderCommand(Guid Id) : IRequest<OrderDto>;
