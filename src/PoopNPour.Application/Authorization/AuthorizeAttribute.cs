namespace PoopNPour.Application.Authorization;

/// <summary>
/// Custom authorization attribute for MediatR requests (Queries/Commands)
/// Supports roles, policies, or both in combination with flexible syntax
/// </summary>
/// <example>
/// <code>
/// // Single role
/// [Authorize("Admin")]
/// 
/// // Multiple roles (OR logic by default)
/// [Authorize(Roles = new[] { "Admin", "Manager" })]
/// 
/// // Comma-separated roles (convenience)
/// [Authorize(Roles = "Admin,Manager")]
/// 
/// // Require ALL roles (AND logic)
/// [Authorize(Roles = new[] { "Admin", "Manager" }, RequireAllRoles = true)]
/// 
/// // Policy-based
/// [Authorize(Policy = "CanViewUsers")]
/// 
/// // Multiple policies
/// [Authorize(Policies = new[] { "CanViewUsers", "CanEditUsers" })]
/// 
/// // Combination
/// [Authorize(Roles = "Admin", Policies = "CanViewUsers")]
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class AuthorizeAttribute : Attribute
{
    private string[]? _roles;
    private string[]? _policies;

    /// <summary>
    /// Roles that are allowed to access this request
    /// Supports both array and comma-separated string
    /// </summary>
    public object? Roles
    {
        get => _roles;
        set
        {
            _roles = value switch
            {
                string str => str.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
                string[] arr => arr,
                _ => null
            };
        }
    }

    /// <summary>
    /// Single policy that must be satisfied
    /// </summary>
    public string? Policy
    {
        get => _policies?.FirstOrDefault();
        set => _policies = value != null ? new[] { value } : null;
    }

    /// <summary>
    /// Multiple policies that must be satisfied
    /// Supports both array and comma-separated string
    /// </summary>
    public object? Policies
    {
        get => _policies;
        set
        {
            _policies = value switch
            {
                string str => str.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
                string[] arr => arr,
                _ => null
            };
        }
    }

    /// <summary>
    /// Require all roles (AND) or any role (OR). Default is OR.
    /// </summary>
    public bool RequireAllRoles { get; set; } = false;

    /// <summary>
    /// Require all policies (AND) or any policy (OR). Default is OR.
    /// </summary>
    public bool RequireAllPolicies { get; set; } = false;

    /// <summary>
    /// Gets the parsed roles array
    /// </summary>
    public string[] GetRoles() => _roles ?? Array.Empty<string>();

    /// <summary>
    /// Gets the parsed policies array
    /// </summary>
    public string[] GetPolicies() => _policies ?? Array.Empty<string>();

    /// <summary>
    /// Default constructor for parameterless authorization (authentication only)
    /// </summary>
    public AuthorizeAttribute()
    {
    }

    /// <summary>
    /// Constructor with single role (convenience)
    /// </summary>
    public AuthorizeAttribute(string role)
    {
        Roles = role;
    }

    /// <summary>
    /// Constructor with multiple roles
    /// </summary>
    public AuthorizeAttribute(params string[] roles)
    {
        Roles = roles;
    }
}
