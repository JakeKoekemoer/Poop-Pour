namespace PoopNPour.Application.Authorization;

/// <summary>
/// Custom authorization attribute for MediatR requests (Queries/Commands)
/// Supports roles, policies, or both in combination
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class AuthorizeAttribute : Attribute
{
    /// <summary>
    /// Roles that are allowed to access this request
    /// </summary>
    public string[]? Roles { get; set; }

    /// <summary>
    /// Policies that must be satisfied to access this request
    /// </summary>
    public string[]? Policies { get; set; }

    /// <summary>
    /// Require all roles (AND) or any role (OR). Default is OR.
    /// </summary>
    public bool RequireAllRoles { get; set; } = false;

    /// <summary>
    /// Require all policies (AND) or any policy (OR). Default is OR.
    /// </summary>
    public bool RequireAllPolicies { get; set; } = false;

    /// <summary>
    /// Authorization with roles only
    /// </summary>
    public AuthorizeAttribute(params string[] roles)
    {
        Roles = roles;
    }

    /// <summary>
    /// Authorization with roles and policies
    /// </summary>
    public AuthorizeAttribute(string[]? roles = null, string[]? policies = null)
    {
        Roles = roles;
        Policies = policies;
    }
}
