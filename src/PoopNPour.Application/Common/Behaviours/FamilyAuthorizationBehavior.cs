using MediatR;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Common.Exceptions;
using PoopNPour.Application.Common.Interfaces;
using System.Reflection;

namespace PoopNPour.Application.Common.Behaviours;

/// <summary>
/// MediatR pipeline behavior that enforces family-scoped role authorization.
/// Runs after the global AuthorizationBehavior.
///
/// Two-stage check:
///   Stage 1 (in-memory): Verifies the FamilyId is present in the user's JWT family_member claims.
///   Stage 2 (DB):        Fetches the live FamilyUser record to get the current role and confirm
///                        the user has not been removed from the family since the token was issued.
///
/// Admin users bypass all checks.
/// Requests must implement IFamilyRequest to supply the FamilyId.
/// </summary>
public class FamilyAuthorizationBehavior<TRequest, TResponse>(
    IUser user,
    IFamilyUserService familyUserService) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var attribute = request.GetType()
            .GetCustomAttribute<AuthorizeFamilyMemberAttribute>();

        if (attribute is null)
            return await next();

        if (user.IsAdmin)
            return await next();

        if (request is not IFamilyRequest familyRequest)
            throw new InvalidOperationException(
                $"{typeof(TRequest).Name} is decorated with [AuthorizeFamilyMember] but does not implement IFamilyRequest.");

        // Stage 1 — in-memory JWT gate (no DB call)
        if (!user.FamilyIds.Contains(familyRequest.FamilyId))
            throw new ForbiddenAccessException(
                $"User is not a member of family '{familyRequest.FamilyId}'.");

        // Stage 2 — live DB lookup to get current role and confirm membership still valid
        if (string.IsNullOrEmpty(user.Id))
            throw new UnauthorizedAccessException("User is not authenticated.");

        var familyUser = await familyUserService.GetFamilyUserByIdAsync(
            familyRequest.FamilyId,
            user.Id,
            cancellationToken);

        if (familyUser is null)
            throw new ForbiddenAccessException(
                $"User has been removed from family '{familyRequest.FamilyId}'.");

        if (familyUser.Role < attribute.MinimumRole)
            throw new ForbiddenAccessException(
                $"Family role '{familyUser.Role}' is insufficient. Required: '{attribute.MinimumRole}'.");

        return await next();
    }
}
