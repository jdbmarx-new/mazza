using FluentValidation;

namespace OrderManagement.Application.Orders.GetPage;

public sealed class GetOrdersValidator : AbstractValidator<GetOrdersPageQuery>
{
    public GetOrdersValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}
