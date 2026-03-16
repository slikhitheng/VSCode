namespace Api.Middleware;

using System.Net;
using System.Text.Json;
using Domain.Exceptions;

/// <summary>
/// Global exception handling middleware for the API
/// </summary>
public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred");
            await HandleExceptionAsync(context, exception);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse
        {
            Message = exception.Message,
            Timestamp = DateTime.UtcNow,
            TraceId = context.TraceIdentifier
        };

        switch (exception)
        {
            case EntityNotFoundException notFound:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                response.Code = notFound.Code;
                break;

            case ValidationException validation:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.Code = validation.Code;
                break;

            case BusinessRuleException businessRule:
                context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                response.Code = businessRule.Code;
                break;

            case DomainException domain:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.Code = domain.Code;
                break;

            case ArgumentNullException:
            case ArgumentException:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.Code = "INVALID_ARGUMENT";
                break;

            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Code = "INTERNAL_SERVER_ERROR";
                response.Message = "An unexpected error occurred. Please try again later.";
                break;
        }

        return context.Response.WriteAsJsonAsync(response);
    }
}

/// <summary>
/// Standard error response model
/// </summary>
public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public string? Code { get; set; }
    public DateTime Timestamp { get; set; }
    public string? TraceId { get; set; }
}

/// <summary>
/// Success response model
/// </summary>
public class SuccessResponse<T>
{
    public bool Success { get; set; } = true;
    public T? Data { get; set; }
    public string Message { get; set; } = "Success";
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Paginated response model
/// </summary>
public class PaginatedResponse<T>
{
    public bool Success { get; set; } = true;
    public IEnumerable<T>? Data { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
