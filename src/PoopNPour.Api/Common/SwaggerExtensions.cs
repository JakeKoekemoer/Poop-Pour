using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PoopNPour.Api.Common;

/// <summary>
/// Swashbuckle operation filter for setting endpoint documentation
/// This is the .NET 10 recommended approach for Swashbuckle users
/// </summary>
public class EndpointDocumentationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Get metadata from endpoint
        var documentationMetadata = context.ApiDescription.ActionDescriptor.EndpointMetadata
            .OfType<EndpointDocumentationMetadata>()
            .FirstOrDefault();

        if (documentationMetadata != null)
        {
            if (!string.IsNullOrWhiteSpace(documentationMetadata.Summary))
            {
                operation.Summary = documentationMetadata.Summary;
            }

            if (!string.IsNullOrWhiteSpace(documentationMetadata.Description))
            {
                operation.Description = documentationMetadata.Description;
            }
        }
    }
}

/// <summary>
/// Metadata to store endpoint documentation
/// </summary>
public class EndpointDocumentationMetadata
{
    public string? Summary { get; set; }
    public string? Description { get; set; }
}

/// <summary>
/// Extension methods for configuring OpenAPI documentation with Swashbuckle
/// </summary>
public static class SwaggerExtensions
{
    /// <summary>
    /// Adds documentation metadata to the endpoint (Swashbuckle-compatible)
    /// This replaces the deprecated .WithOpenApi() approach
    /// </summary>
    public static RouteHandlerBuilder WithDocumentation(
        this RouteHandlerBuilder builder,
        string summary,
        string? description = null)
    {
        return builder.WithMetadata(new EndpointDocumentationMetadata
        {
            Summary = summary,
            Description = description
        });
    }

    /// <summary>
    /// Adds documentation metadata to the endpoint - summary only
    /// </summary>
    public static RouteHandlerBuilder WithSummaryV2(
        this RouteHandlerBuilder builder,
        string summary)
    {
        return builder.WithDocumentation(summary);
    }

    /// <summary>
    /// Configures request and response types for an endpoint that accepts and produces JSON
    /// </summary>
    /// <typeparam name="TRequest">The request DTO type</typeparam>
    /// <typeparam name="TResponse">The response DTO type</typeparam>
    /// <param name="builder">The route handler builder</param>
    /// <param name="statusCode">The success status code (default: 200 OK)</param>
    /// <returns>The route handler builder for method chaining</returns>
    public static RouteHandlerBuilder WithRequestResponse<TRequest, TResponse>(
        this RouteHandlerBuilder builder,
        int statusCode = StatusCodes.Status200OK)
        where TRequest : notnull
    {
        return builder
            .Accepts<TRequest>("application/json")
            .Produces<TResponse>(statusCode);
    }

    /// <summary>
    /// Configures request type for an endpoint that accepts JSON but returns no content
    /// </summary>
    /// <typeparam name="TRequest">The request DTO type</typeparam>
    /// <param name="builder">The route handler builder</param>
    /// <param name="statusCode">The success status code (default: 200 OK)</param>
    /// <returns>The route handler builder for method chaining</returns>
    public static RouteHandlerBuilder WithRequest<TRequest>(
        this RouteHandlerBuilder builder,
        int statusCode = StatusCodes.Status200OK)
        where TRequest : notnull
    {
        return builder
            .Accepts<TRequest>("application/json")
            .Produces(statusCode);
    }

    /// <summary>
    /// Configures response type for an endpoint that produces JSON
    /// </summary>
    /// <typeparam name="TResponse">The response DTO type</typeparam>
    /// <param name="builder">The route handler builder</param>
    /// <param name="statusCode">The success status code (default: 200 OK)</param>
    /// <returns>The route handler builder for method chaining</returns>
    public static RouteHandlerBuilder WithResponse<TResponse>(
        this RouteHandlerBuilder builder,
        int statusCode = StatusCodes.Status200OK)
    {
        return builder.Produces<TResponse>(statusCode);
    }
}
