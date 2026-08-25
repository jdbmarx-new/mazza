using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Common;
using OrderManagement.Domain.Common;

namespace OrderManagement.Api;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled request error");

            (int status, string? title, string? detail) = ex switch
            {
                ValidationException => (400, "Validation failed", string.Join("; ", ((ValidationException)ex).Errors.Select(e => e.ErrorMessage))),
                NotFoundException => (404, "Resource not found", ex.Message),
                ConflictException or DomainException => (409, "Business rule violation", ex.Message),
                _ => (500, "Unexpected error", "An unexpected error occurred.")
            };

            context.Response.StatusCode = status;

            await context.Response.WriteAsJsonAsync(
                new ProblemDetails
                {
                    Status = status,
                    Title = title,
                    Detail = detail,
                    Instance = context.Request.Path
                }, context.RequestAborted);
        }
    }
}