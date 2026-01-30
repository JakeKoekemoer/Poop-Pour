namespace PoopNPour.Domain.Common.Auth;

/// <summary>
/// Application policies constants
/// </summary>
public static class Policies
{
    public const string RequireAdministrator = nameof(RequireAdministrator);
    public const string RequireUser = nameof(RequireUser);
    
    // Add your custom policies here
    // Example:
    // public const string CanManageProducts = nameof(CanManageProducts);
    // public const string CanViewReports = nameof(CanViewReports);
}
