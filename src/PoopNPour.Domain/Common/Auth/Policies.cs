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

    #endregion User Policies
}
