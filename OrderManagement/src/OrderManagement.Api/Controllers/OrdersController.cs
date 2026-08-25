using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Common;
using OrderManagement.Application.Orders.Cancel;
using OrderManagement.Application.Orders.Create;
using OrderManagement.Application.Orders.GetById;
using OrderManagement.Application.Orders.GetPage;

namespace OrderManagement.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/orders")]
public sealed class OrdersController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create(CreateOrderRequest r, CancellationToken cancellationToken)
    {
        OrderDto result = await sender.Send(new CreateOrderCommand(r.CustomerId,
                                                              [.. r.Items.Select(i => new CreateOrderItem(i.ProductName, i.Quantity, i.UnitPrice))]),
                                                              cancellationToken);
        return CreatedAtAction(nameof(GetById),
                               new
                               {
                                   id = result.Id
                               },
                               result);
    }

    [HttpGet]
    public Task<PagedResult<OrderDto>> GetPage([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
    {
        return sender.Send(new GetOrdersPageQuery(page, pageSize), ct);
    }

    [HttpGet("{id:guid}")]
    public Task<OrderDto> GetById(Guid id, CancellationToken cancellationToken)
    {
        return sender.Send(new GetOrderByIdQuery(id), cancellationToken);
    }

    [HttpPatch("{id:guid}/cancel")]
    public Task<OrderDto> Cancel(Guid id, CancellationToken cancellationToken)
    {
        return sender.Send(new CancelOrderCommand(id), cancellationToken);
    }
}