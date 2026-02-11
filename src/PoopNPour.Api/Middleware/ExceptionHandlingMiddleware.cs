using PoopNPour.Application.Common.Exceptions;
using PoopNPour.Application.Users.Exceptions;
using PoopNPour.Application.Authentication.Exceptions;
using System.Net;
using System.Text.Json;

namespace PoopNPour.Api.Middleware;

/// <summary>
/// Middleware that translates application exceptions to HTTP responses
/// This is the ONLY place where we map domain/application exceptions to HTTP status codes
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = exception switch
        {
            // Authorization exceptions (from Application layer)
            UnauthorizedAccessException => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
                Message = "Unauthorized",
                Detail = exception.Message
            },

            ForbiddenAccessException => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.Forbidden,
                Message = "Forbidden",
                Detail = exception.Message
            },

            // User exceptions (from Application layer)
            UserNotFoundException => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Not Found",
                Detail = exception.Message
            },

            EmailAlreadyInUseException => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = "Bad Request",
                Detail = exception.Message
            },

            // Authentication exceptions (from Application layer)
            InvalidCredentialsException => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
                Message = "Unauthorized",
                Detail = exception.Message
            },

            UserAlreadyExistsException => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = "Bad Request",
                Detail = exception.Message
            },

            // Validation exceptions (from Application layer)
            Application.Common.Exceptions.ValidationException validationEx => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = "Validation Failed",
                Detail = validationEx.Message,
                Errors = validationEx.Errors
            },

            // Default for unhandled exceptions
            _ => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "Internal Server Error",
                Detail = "An unexpected error occurred."
            }
        };

        // Log at API layer (HTTP context available here)
        if (errorResponse.StatusCode >= 500)
        {
            _logger.LogError(exception, "Server error occurred while processing request to {Path}", context.Request.Path);
        }
        else if (errorResponse.StatusCode == 401 || errorResponse.StatusCode == 403)
        {
            _logger.LogWarning("Authorization failed for request to {Path}: {Message}", context.Request.Path, exception.Message);
        }
        else
        {
            _logger.LogInformation("Client error occurred for request to {Path}: {Message}", context.Request.Path, exception.Message);
        }

        response.StatusCode = errorResponse.StatusCode;
        await response.WriteAsync(JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));
    }
}

/// <summary>
/// Standard error response format for HTTP responses
/// </summary>
public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public Dictionary<string, string[]>? Errors { get; set; }
}
