using MediatR;
using OrderManagement.Application.Abstractions;
using OrderManagement.Application.Common;
using OrderManagement.Domain.Orders;

namespace OrderManagement.Application.Orders.GetPage;

public sealed class GetOrdersHandler(IOrderRepository repository) : IRequestHandler<GetOrdersPageQuery, PagedResult<OrderDto>>
{
    public async Task<PagedResult<OrderDto>> Handle(GetOrdersPageQuery r, CancellationToken cancellationToken)
    {
        (IReadOnlyList<Order> Items, int Total) result = await repository.GetPageAsync(r.Page, r.PageSize, cancellationToken);

        return new([..
                        result.Items.Select(OrderDto.From)
                   ], r.Page, r.PageSize, result.Total);
    }
}