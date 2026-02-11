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
}
