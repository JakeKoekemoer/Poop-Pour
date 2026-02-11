namespace PoopNPour.Api.Middleware;

/// <summary>
/// Extension methods for registering API middleware
/// </summary>
public static class MiddlewareExtensions
{
    /// <summary>
    /// Adds global exception handling middleware to the HTTP pipeline
    /// This middleware translates Application layer exceptions to HTTP responses
    /// </summary>
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
