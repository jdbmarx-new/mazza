using System.Diagnostics;

using MediatR;
using Microsoft.Extensions.Logging;

namespace OrderManagement.Application.Common.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        logger.LogInformation("Handling {RequestName}: {@Request}", typeof(TRequest).Name, request);

#pragma warning disable S2139
        try
        {
            TResponse? response = await next(cancellationToken);
            logger.LogInformation("Handled {RequestName} in {ElapsedMs} ms: {@Response}", typeof(TRequest).Name, sw.ElapsedMilliseconds, response);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed {RequestName} after {ElapsedMs} ms", typeof(TRequest).Name, sw.ElapsedMilliseconds);
            throw;
        }
#pragma warning restore S2139
    }
}