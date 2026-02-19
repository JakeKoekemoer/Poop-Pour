using System.Reflection;
using Microsoft.Extensions.Logging;

namespace PoopNPour.Api.Common;

/// <summary>
/// Extension methods for WebApplication
/// Following the Pipeline-X pattern
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Creates a route group for the endpoint class, using the class name as the route prefix
    /// Convention: Class name becomes the route (e.g., "Users" -> "/api/users")
    /// </summary>
    public static RouteGroupBuilder MapGroup(this WebApplication app, EndpointGroupBase group)
    {
        var groupName = group.GetType().Name;
        
        // Convert PascalCase to lowercase (e.g., "UsersEndpoints" -> "users")
        var routeName = ConvertToRouteName(groupName);

        // Use friendly tag name (e.g. "Authentication") instead of class name ("AuthenticationEndpoints")
        var tagName = ToDisplayName(groupName);
        return app.MapGroup($"/api/{routeName}")
            .WithTags(tagName);
    }

    /// <summary>
    /// Automatically discovers and registers all endpoint groups that inherit from EndpointGroupBase
    /// </summary>
    public static WebApplication MapEndpointGroups(this WebApplication app)
    {
        var endpointGroupType = typeof(EndpointGroupBase);
        
        // Get the assembly where EndpointGroupBase is defined (the API assembly)
        var assembly = endpointGroupType.Assembly;

        var endpointGroupTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(endpointGroupType))
            .OrderBy(t => t.Name)
            .ToList();
        
        var logger = app.Services.GetRequiredService<ILogger<WebApplication>>();
        logger.LogInformation("Discovered {Count} endpoint groups", endpointGroupTypes.Count);

        foreach (var type in endpointGroupTypes)
        {
            logger.LogInformation("Mapping endpoint group: {TypeName}", type.Name);
            
            try
            {
                if (Activator.CreateInstance(type) is EndpointGroupBase instance)
                {
                    instance.Map(app);
                    logger.LogInformation("Successfully mapped endpoint group: {TypeName}", type.Name);
                }
                else
                {
                    logger.LogWarning("Failed to create instance of {TypeName}", type.Name);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error mapping endpoint group {TypeName}", type.Name);
            }
        }

        return app;
    }

    /// <summary>
    /// Converts a class name to a display-friendly tag name for Swagger.
    /// Examples: "AuthenticationEndpoints" -> "Authentication", "UsersEndpoints" -> "Users", "FeedLogsEndpoints" -> "Feed Logs"
    /// </summary>
    private static string ToDisplayName(string className)
    {
        if (className.EndsWith("Endpoints", StringComparison.OrdinalIgnoreCase))
        {
            className = className.Substring(0, className.Length - "Endpoints".Length);
        }
        
        // Insert spaces before capital letters (PascalCase to Title Case)
        var result = System.Text.RegularExpressions.Regex.Replace(className, "([A-Z])", " $1").Trim();
        return result;
    }

    /// <summary>
    /// Converts a class name to a route name
    /// Examples: "UsersEndpoints" -> "users", "Users" -> "users", "AuthenticationEndpoints" -> "authentication", "FeedLogsEndpoints" -> "feed-logs"
    /// </summary>
    private static string ConvertToRouteName(string className)
    {
        // Remove "Endpoints" suffix if present
        if (className.EndsWith("Endpoints", StringComparison.OrdinalIgnoreCase))
        {
            className = className.Substring(0, className.Length - "Endpoints".Length);
        }

        // Convert PascalCase to kebab-case (e.g., "FeedLogs" -> "feed-logs")
        var kebabCase = System.Text.RegularExpressions.Regex.Replace(className, "(?<!^)([A-Z])", "-$1");

        // Convert to lowercase
        return kebabCase.ToLowerInvariant();
    }

}
