using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using PoopNPour.Application.Authorization;
using PoopNPour.Domain.Common.Auth;
using System.Security.Claims;

namespace PoopNPour.Application.Authorization;

/// <summary>
/// MediatR pipeline behavior that enforces authorization based on AuthorizeAttribute
/// </summary>
public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthorizationService _authorizationService;

    public AuthorizationBehavior(
        IHttpContextAccessor httpContextAccessor,
        IAuthorizationService authorizationService)
    {
        _httpContextAccessor = httpContextAccessor;
        _authorizationService = authorizationService;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Check if the request has an Authorize attribute
        var authorizeAttribute = typeof(TRequest)
            .GetCustomAttributes(typeof(AuthorizeAttribute), true)
            .FirstOrDefault() as AuthorizeAttribute;

        if (authorizeAttribute == null)
        {
            // No authorization required, proceed
            return await next();
        }

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new UnauthorizedAccessException("HTTP context is not available.");
        }

        var user = httpContext.User;
        if (user?.Identity?.IsAuthenticated != true)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        // Check roles
        if (authorizeAttribute.Roles != null && authorizeAttribute.Roles.Length > 0)
        {
            var userRoles = user.Claims
                .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            var hasRequiredRoles = authorizeAttribute.RequireAllRoles
                ? authorizeAttribute.Roles.All(role => userRoles.Contains(role))
                : authorizeAttribute.Roles.Any(role => userRoles.Contains(role));

            if (!hasRequiredRoles)
            {
                throw new UnauthorizedAccessException(
                    $"User does not have required role(s): {string.Join(", ", authorizeAttribute.Roles)}");
            }
        }

        // Check policies
        if (authorizeAttribute.Policies != null && authorizeAttribute.Policies.Length > 0)
        {
            var policyResults = new List<AuthorizationResult>();

            foreach (var policyName in authorizeAttribute.Policies)
            {
                // Authorize using the configured policy name
                var result = await _authorizationService.AuthorizeAsync(user, policyName);
                policyResults.Add(result);
            }

            var hasRequiredPolicies = authorizeAttribute.RequireAllPolicies
                ? policyResults.All(r => r.Succeeded)
                : policyResults.Any(r => r.Succeeded);

            if (!hasRequiredPolicies)
            {
                throw new UnauthorizedAccessException(
                    $"User does not satisfy required policy/policies: {string.Join(", ", authorizeAttribute.Policies)}");
            }
        }

        // Authorization passed, proceed
        return await next();
    }
}

/// <summary>
/// Policy requirement for custom policies
/// </summary>
public class PolicyRequirement : IAuthorizationRequirement
{
    public string PolicyName { get; }

    public PolicyRequirement(string policyName)
    {
        PolicyName = policyName;
    }
}

/// <summary>
/// Policy handler for custom policies
/// </summary>
public class PolicyRequirementHandler : AuthorizationHandler<PolicyRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PolicyRequirement requirement)
    {
        // Check if user has a claim for this policy
        var hasPolicy = context.User.HasClaim(Domain.Common.Auth.ClaimTypes.Policy, requirement.PolicyName) ||
                       context.User.HasClaim(System.Security.Claims.ClaimTypes.Role, requirement.PolicyName);

        if (hasPolicy)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
