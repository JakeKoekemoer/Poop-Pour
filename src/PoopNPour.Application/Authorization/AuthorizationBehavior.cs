using MediatR;
using PoopNPour.Application.Common.Exceptions;
using PoopNPour.Abstractions.Identity;
using System.Reflection;

namespace PoopNPour.Application.Authorization;

/// <summary>
/// MediatR pipeline behavior that enforces authorization based on AuthorizeAttribute
/// Combines best practices from both Pipeline-X and Poop-Pour patterns
/// </summary>
public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IUser _user;
    private readonly IIdentityService _identityService;

    public AuthorizationBehavior(
        IUser user,
        IIdentityService identityService)
    {
        _user = user;
        _identityService = identityService;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Check if the request has an Authorize attribute
        var authorizeAttributes = request.GetType()
            .GetCustomAttributes<AuthorizeAttribute>()
            .ToList();

        if (!authorizeAttributes.Any())
        {
            // No authorization required, proceed
            return await next();
        }

        // Must be authenticated user
        if (!_user.IsAuthenticated || string.IsNullOrEmpty(_user.Id))
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        // Process all authorize attributes
        foreach (var authorizeAttribute in authorizeAttributes)
        {
            await ValidateRolesAsync(authorizeAttribute);
            await ValidatePoliciesAsync(authorizeAttribute);
        }

        // User is authorized, proceed
        return await next();
    }

    private async Task ValidateRolesAsync(AuthorizeAttribute authorizeAttribute)
    {
        var roles = authorizeAttribute.GetRoles();
        if (!roles.Any())
            return;

        var authorized = false;

        if (authorizeAttribute.RequireAllRoles)
        {
            // AND logic: user must have ALL roles
            authorized = true;
            foreach (var role in roles)
            {
                var isInRole = await _identityService.IsInRoleAsync(_user.Id!, role);
                if (!isInRole)
                {
                    authorized = false;
                    break;
                }
            }
        }
        else
        {
            // OR logic: user must have AT LEAST ONE role
            foreach (var role in roles)
            {
                var isInRole = await _identityService.IsInRoleAsync(_user.Id!, role);
                if (isInRole)
                {
                    authorized = true;
                    break;
                }
            }
        }

        if (!authorized)
        {
            throw new ForbiddenAccessException(
                $"User does not have required role(s): {string.Join(", ", roles)}");
        }
    }

    private async Task ValidatePoliciesAsync(AuthorizeAttribute authorizeAttribute)
    {
        var policies = authorizeAttribute.GetPolicies();
        if (!policies.Any())
            return;

        var authorized = false;

        if (authorizeAttribute.RequireAllPolicies)
        {
            // AND logic: user must satisfy ALL policies
            authorized = true;
            foreach (var policy in policies)
            {
                var satisfiesPolicy = await _identityService.AuthorizeAsync(_user.Id!, policy);
                if (!satisfiesPolicy)
                {
                    authorized = false;
                    break;
                }
            }
        }
        else
        {
            // OR logic: user must satisfy AT LEAST ONE policy
            foreach (var policy in policies)
            {
                var satisfiesPolicy = await _identityService.AuthorizeAsync(_user.Id!, policy);
                if (satisfiesPolicy)
                {
                    authorized = true;
                    break;
                }
            }
        }

        if (!authorized)
        {
            throw new ForbiddenAccessException(
                $"User does not satisfy required policy/policies: {string.Join(", ", policies)}");
        }
    }
}
