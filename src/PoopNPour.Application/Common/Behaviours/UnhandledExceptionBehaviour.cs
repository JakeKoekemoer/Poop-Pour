using MediatR;
using Microsoft.Extensions.Logging;

namespace PoopNPour.Application.Common.Behaviours;

/// <summary>
/// Pipeline behavior that catches and logs unhandled exceptions
/// </summary>
public class UnhandledExceptionBehaviour<TRequest, TResponse>(ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;

            logger.LogError(ex, "Unhandled Exception for Request {Name} {@Request}", requestName, request);

            // Re-throw to let the API middleware handle HTTP concerns
            throw;
        }
    }
}
