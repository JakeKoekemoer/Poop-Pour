using PoopNPour.Application.Common.Exceptions;
using PoopNPour.Application.Users.Exceptions;
using PoopNPour.Application.Authentication.Exceptions;
using PoopNPour.Application.Families.Exceptions;
using PoopNPour.Application.FamilyUsers.Exceptions;
using PoopNPour.Application.Dependents.Exceptions;
using PoopNPour.Application.DiperLogs.Exceptions;
using PoopNPour.Application.FeedLogs.Exceptions;
using PoopNPour.Application.MedicineLogs.Exceptions;
using System.Net;
using System.Text.Json;

namespace PoopNPour.Api.Middleware;

/// <summary>
/// Middleware that translates application exceptions to HTTP responses
/// This is the ONLY place where we map domain/application exceptions to HTTP status codes
/// </summary>
public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
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

            // All NotFoundException derived exceptions (base catch-all)
            NotFoundException => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Not Found",
                Detail = exception.Message
            },

            // All DuplicateException derived exceptions (base catch-all for conflicts/duplicates)
            DuplicateException => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = "Bad Request",
                Detail = exception.Message
            },

            // All CreateFailedException derived exceptions (base catch-all for creation failures)
            CreateFailedException => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = "Creation Failed",
                Detail = exception.Message
            },

            // Authentication exceptions (from Application layer)
            InvalidCredentialsException => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
                Message = "Unauthorized",
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
            logger.LogError(exception, "Server error occurred while processing request to {Path}", context.Request.Path);
        }
        else if (errorResponse.StatusCode == 401 || errorResponse.StatusCode == 403)
        {
            logger.LogWarning("Authorization failed for request to {Path}: {Message}", context.Request.Path, exception.Message);
        }
        else
        {
            logger.LogInformation("Client error occurred for request to {Path}: {Message}", context.Request.Path, exception.Message);
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
