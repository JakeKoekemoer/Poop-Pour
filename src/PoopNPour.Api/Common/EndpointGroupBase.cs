namespace PoopNPour.Api.Common;

/// <summary>
/// Base class for endpoint groups that can be automatically discovered and registered
/// </summary>
public abstract class EndpointGroupBase
{
    /// <summary>
    /// Maps the endpoints for this group
    /// </summary>
    /// <param name="app">The web application</param>
    public abstract void Map(WebApplication app);
}
