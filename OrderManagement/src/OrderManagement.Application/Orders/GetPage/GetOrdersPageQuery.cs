using MediatR;
using OrderManagement.Application.Common;

namespace OrderManagement.Application.Orders.GetPage;

public sealed record GetOrdersPageQuery(int Page = 1, int PageSize = 10) : IRequest<PagedResult<OrderDto>>;
