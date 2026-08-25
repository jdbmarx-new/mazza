using MediatR;
using OrderManagement.Application.Common;

namespace OrderManagement.Application.Orders.GetById;

public sealed record GetOrderByIdQuery(Guid Id) : IRequest<OrderDto>;
