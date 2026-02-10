namespace PoopNPour.Api.Common;

/// <summary>
/// Extension methods for IEndpointRouteBuilder that enable fluent chaining
/// Following the Pipeline-X pattern
/// </summary>
public static class EndpointRouteBuilderExtensions
{
    /// <summary>
    /// Maps a GET endpoint and returns the builder for chaining
    /// </summary>
    public static IEndpointRouteBuilder MapGet(
        this IEndpointRouteBuilder builder,
        Delegate handler,
        string pattern = "")
    {
        var routeBuilder = builder.MapGet(pattern, handler);
        
        // Use the method name as the endpoint name
        routeBuilder.WithName(handler.Method.Name);
        
        return builder;
    }

    /// <summary>
    /// Maps a GET endpoint with configuration and returns the builder for chaining
    /// </summary>
    public static IEndpointRouteBuilder MapGet(
        this IEndpointRouteBuilder builder,
        Delegate handler,
        string pattern,
        Action<RouteHandlerBuilder> configure)
    {
        var routeBuilder = builder.MapGet(pattern, handler);
        routeBuilder.WithName(handler.Method.Name);
        configure?.Invoke(routeBuilder);
        return builder;
    }

    /// <summary>
    /// Maps a POST endpoint and returns the builder for chaining
    /// </summary>
    public static IEndpointRouteBuilder MapPost(
        this IEndpointRouteBuilder builder,
        Delegate handler,
        string pattern = "")
    {
        var routeBuilder = builder.MapPost(pattern, handler);
        routeBuilder.WithName(handler.Method.Name);
        return builder;
    }

    /// <summary>
    /// Maps a POST endpoint with configuration and returns the builder for chaining
    /// </summary>
    public static IEndpointRouteBuilder MapPost(
        this IEndpointRouteBuilder builder,
        Delegate handler,
        string pattern,
        Action<RouteHandlerBuilder> configure)
    {
        var routeBuilder = builder.MapPost(pattern, handler);
        routeBuilder.WithName(handler.Method.Name);
        configure?.Invoke(routeBuilder);
        return builder;
    }

    /// <summary>
    /// Maps a PUT endpoint and returns the builder for chaining
    /// </summary>
    public static IEndpointRouteBuilder MapPut(
        this IEndpointRouteBuilder builder,
        Delegate handler,
        string pattern)
    {
        var routeBuilder = builder.MapPut(pattern, handler);
        routeBuilder.WithName(handler.Method.Name);
        return builder;
    }

    /// <summary>
    /// Maps a PUT endpoint with configuration and returns the builder for chaining
    /// </summary>
    public static IEndpointRouteBuilder MapPut(
        this IEndpointRouteBuilder builder,
        Delegate handler,
        string pattern,
        Action<RouteHandlerBuilder> configure)
    {
        var routeBuilder = builder.MapPut(pattern, handler);
        routeBuilder.WithName(handler.Method.Name);
        configure?.Invoke(routeBuilder);
        return builder;
    }

    /// <summary>
    /// Maps a DELETE endpoint and returns the builder for chaining
    /// </summary>
    public static IEndpointRouteBuilder MapDelete(
        this IEndpointRouteBuilder builder,
        Delegate handler,
        string pattern)
    {
        var routeBuilder = builder.MapDelete(pattern, handler);
        routeBuilder.WithName(handler.Method.Name);
        return builder;
    }

    /// <summary>
    /// Maps a DELETE endpoint with configuration and returns the builder for chaining
    /// </summary>
    public static IEndpointRouteBuilder MapDelete(
        this IEndpointRouteBuilder builder,
        Delegate handler,
        string pattern,
        Action<RouteHandlerBuilder> configure)
    {
        var routeBuilder = builder.MapDelete(pattern, handler);
        routeBuilder.WithName(handler.Method.Name);
        configure?.Invoke(routeBuilder);
        return builder;
    }

    /// <summary>
    /// Maps a PATCH endpoint and returns the builder for chaining
    /// </summary>
    public static IEndpointRouteBuilder MapPatch(
        this IEndpointRouteBuilder builder,
        Delegate handler,
        string pattern)
    {
        var routeBuilder = builder.MapPatch(pattern, handler);
        routeBuilder.WithName(handler.Method.Name);
        return builder;
    }

    /// <summary>
    /// Maps a PATCH endpoint with configuration and returns the builder for chaining
    /// </summary>
    public static IEndpointRouteBuilder MapPatch(
        this IEndpointRouteBuilder builder,
        Delegate handler,
        string pattern,
        Action<RouteHandlerBuilder> configure)
    {
        var routeBuilder = builder.MapPatch(pattern, handler);
        routeBuilder.WithName(handler.Method.Name);
        configure?.Invoke(routeBuilder);
        return builder;
    }
}
