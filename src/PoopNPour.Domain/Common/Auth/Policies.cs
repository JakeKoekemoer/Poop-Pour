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

    #endregion User Management Policies
}
