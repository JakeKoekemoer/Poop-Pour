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
                Roles.WebApi,
                Roles.MobileApi
            ])
        );

        options.AddPolicy(Policies.AccountForgotPassword,
            policy => policy.RequireRole([
                Roles.WebApi,
                Roles.MobileApi
            ])
        );

        options.AddPolicy(Policies.AccountLogin,
            policy => policy.RequireRole([
                Roles.WebApi,
                Roles.MobileApi
            ])
        );

        #endregion User Management Policies

        return options;
    }
}
