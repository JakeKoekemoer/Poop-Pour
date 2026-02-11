using Microsoft.AspNetCore.Authorization;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Infrastructure.Authorization;

/// <summary>
/// Centralized configuration for mapping policies to roles.
/// This file defines which roles satisfy which authorization policies.
/// </summary>
public static class PolicyRoleMappings
{
    /// <summary>
    /// Adds roles to authorization policies.
    /// </summary>
    /// <param name="options">The authorization options to configure</param>
    /// <returns>The configured authorization options</returns>
    public static AuthorizationOptions AddRolesToPolicies(AuthorizationOptions options)
    {
        #region User Management Policies

        options.AddPolicy(Policies.AccountRegistration,
            policy => policy.RequireRole([
                Roles.Web_Api,
                Roles.Mobile_Api
            ])
        );

        options.AddPolicy(Policies.AccountForgotPassword,
            policy => policy.RequireRole([
                Roles.Web_Api,
                Roles.Mobile_Api
            ])
        );

        options.AddPolicy(Policies.AccountLogin,
            policy => policy.RequireRole([
                Roles.Web_Api,
                Roles.Mobile_Api
            ])
        );

        options.AddPolicy(Policies.Can_AuthenticateUser,
            policy => policy.RequireRole([
                Roles.Web_Api,
                Roles.Mobile_Api
            ])
        );

        options.AddPolicy(Policies.Can_ManageOwnProfile,
            policy => policy.RequireRole([
                Roles.Family_Head,
                Roles.Family_Member,
                Roles.Administrator
            ])
        );

        options.AddPolicy(Policies.CanViewUsers,
            policy => policy.RequireRole(Roles.Administrator));

        options.AddPolicy(Policies.CanManageUsers,
            policy => policy.RequireRole(Roles.Administrator));

        #endregion User Management Policies

        #region Settings Policies

        options.AddPolicy(Policies.CanManageSystemSettings,
            policy => policy.RequireRole(Roles.Administrator));

        #endregion Settings Policies

        return options;
    }
}
