using System.Reflection;
using PoopNPour.Application.Authorization;

namespace PoopNPour.Api.Common;

/// <summary>
/// Extension methods for RouteHandlerBuilder that automatically configure common scenarios
/// </summary>
public static class RouteHandlerExtensions
{
    /// <summary>
    /// Automatically configures Produces based on the request type's authorization requirements
    /// and common response patterns
    /// </summary>
    /// <typeparam name="TRequest">The MediatR request type (Command/Query)</typeparam>
    /// <typeparam name="TResponse">The response type</typeparam>
    public static RouteHandlerBuilder ProducesStandard<TRequest, TResponse>(
        this RouteHandlerBuilder builder,
        params int[] additionalStatusCodes)
    {
        var requestType = typeof(TRequest);
        var responseType = typeof(TResponse);

        // Always produce success response
        builder.Produces<TResponse>(StatusCodes.Status200OK);

        var authorizeAttribute = requestType.GetCustomAttribute<AuthorizeAttribute>();
        
        if (authorizeAttribute != null)
        {
            // Has authorization - add 401 and potentially 403
            builder.Produces(StatusCodes.Status401Unauthorized);
            
            // If it has roles or policies, user could be authenticated but forbidden
            if ((authorizeAttribute.GetRoles()?.Any() ?? false) || 
                (authorizeAttribute.GetPolicies()?.Any() ?? false))
            {
                builder.Produces(StatusCodes.Status403Forbidden);
            }
        }

        foreach (var statusCode in additionalStatusCodes)
        {
            builder.Produces(statusCode);
        }

        return builder;
    }

    /// <summary>
    /// Configures standard response codes for a query that returns a single item
    /// Automatically adds 404 Not Found in addition to authorization codes
    /// </summary>
    public static RouteHandlerBuilder ProducesSingleItem<TRequest, TResponse>(
        this RouteHandlerBuilder builder)
        where TRequest : class
    {
        return builder.ProducesStandard<TRequest, TResponse>(StatusCodes.Status404NotFound);
    }

    /// <summary>
    /// Configures standard response codes for a command that creates a resource
    /// Uses 201 Created instead of 200 OK
    /// </summary>
    public static RouteHandlerBuilder ProducesCreated<TRequest, TResponse>(
        this RouteHandlerBuilder builder,
        params int[] additionalStatusCodes)
        where TRequest : class
    {
        var requestType = typeof(TRequest);

        builder.Produces<TResponse>(StatusCodes.Status201Created);

        var authorizeAttribute = requestType.GetCustomAttribute<AuthorizeAttribute>();
        if (authorizeAttribute != null)
        {
            builder.Produces(StatusCodes.Status401Unauthorized);
            
            if ((authorizeAttribute.GetRoles()?.Any() ?? false) || 
                (authorizeAttribute.GetPolicies()?.Any() ?? false))
            {
                builder.Produces(StatusCodes.Status403Forbidden);
            }
        }

        builder.Produces(StatusCodes.Status400BadRequest);

        foreach (var statusCode in additionalStatusCodes)
        {
            builder.Produces(statusCode);
        }

        return builder;
    }

    /// <summary>
    /// Configures standard response codes for a command/query that modifies data
    /// Includes 400 Bad Request for validation errors
    /// </summary>
    public static RouteHandlerBuilder ProducesModified<TRequest, TResponse>(
        this RouteHandlerBuilder builder,
        params int[] additionalStatusCodes)
        where TRequest : class
    {
        var allStatusCodes = new List<int> { StatusCodes.Status400BadRequest };
        allStatusCodes.AddRange(additionalStatusCodes);

        return builder.ProducesStandard<TRequest, TResponse>(allStatusCodes.ToArray());
    }
}
