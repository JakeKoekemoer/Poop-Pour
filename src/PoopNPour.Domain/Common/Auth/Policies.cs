namespace PoopNPour.Domain.Common.Auth;

/// <summary>
/// Application policies constants
/// </summary>
public static class Policies
{
    #region User Policies

    public const string AccountRegistration = nameof(AccountRegistration);
    public const string AccountForgotPassword = nameof(AccountForgotPassword);
    public const string AccountLogin = nameof(AccountLogin);
    public const string Can_AuthenticateUser = nameof(Can_AuthenticateUser);
    public const string Can_ManageOwnProfile = nameof(Can_ManageOwnProfile);

    #endregion User Policies

    #region User Management Policies

    public const string CanViewUsers = nameof(CanViewUsers);
    public const string CanManageUsers = nameof(CanManageUsers);
    public const string CanViewUserClaims = nameof(CanViewUserClaims);
    public const string CanManageUserClaims = nameof(CanManageUserClaims);

    #endregion User Management Policies

    #region Settings Policies

    public const string CanManageSystemSettings = nameof(CanManageSystemSettings);

    #endregion Settings Policies

    #region Family Policies

    public const string CanManageFamilies = nameof(CanManageFamilies);
    public const string CanViewFamilies = nameof(CanViewFamilies);

    #endregion Family Policies

    #region Dependent Policies

    public const string CanManageDependents = nameof(CanManageDependents);
    public const string CanViewDependents = nameof(CanViewDependents);

    #endregion Dependent Policies

    #region Log Policies

    public const string CanManageLogs = nameof(CanManageLogs);
    public const string CanViewLogs = nameof(CanViewLogs);

    #endregion Log Policies
}
