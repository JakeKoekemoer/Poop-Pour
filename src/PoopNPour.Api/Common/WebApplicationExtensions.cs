using System.Reflection;

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

        return app.MapGroup($"/api/{routeName}")
            .WithGroupName(groupName)
            .WithTags(groupName)
            .WithOpenApi();
    }

    /// <summary>
    /// Automatically discovers and registers all endpoint groups that inherit from EndpointGroupBase
    /// </summary>
    public static WebApplication MapEndpointGroups(this WebApplication app)
    {
        var endpointGroupType = typeof(EndpointGroupBase);
        var assembly = Assembly.GetExecutingAssembly();

        var endpointGroupTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(endpointGroupType))
            .OrderBy(t => t.Name);

        foreach (var type in endpointGroupTypes)
        {
            if (Activator.CreateInstance(type) is EndpointGroupBase instance)
            {
                instance.Map(app);
            }
        }

        return app;
    }

    /// <summary>
    /// Converts a class name to a route name
    /// Examples: "UsersEndpoints" -> "users", "Users" -> "users", "AuthenticationEndpoints" -> "authentication"
    /// </summary>
    private static string ConvertToRouteName(string className)
    {
        // Remove "Endpoints" suffix if present
        if (className.EndsWith("Endpoints", StringComparison.OrdinalIgnoreCase))
        {
            className = className.Substring(0, className.Length - "Endpoints".Length);
        }

        // Convert to lowercase
        return className.ToLowerInvariant();
    }
}
