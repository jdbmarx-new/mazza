using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace OrderManagement.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            ValidationResult[] results = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            ValidationFailure[] failures = [..
                                                results.SelectMany(x => x.Errors).Where(x => x is not null)
                                           ];
            if (failures.Length > 0)
            {
                throw new ValidationException(failures);
            }
        }
        return await next(cancellationToken);
    }
}