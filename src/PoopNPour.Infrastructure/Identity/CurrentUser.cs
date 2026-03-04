using Microsoft.AspNetCore.Http;
using PoopNPour.Abstractions.Identity;
using System.Security.Claims;
using DomainClaimTypes = PoopNPour.Domain.Common.Auth.ClaimTypes;

namespace PoopNPour.Infrastructure.Identity;

/// <summary>
/// Implementation of IUser that retrieves current user information from HttpContext
/// </summary>
public class CurrentUser(IHttpContextAccessor httpContextAccessor) : IUser
{
    public string? Id => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? UserName => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

    public string? Email => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public bool IsAdmin => httpContextAccessor.HttpContext?.User?.IsInRole(Domain.Common.Auth.Roles.Administrator) ?? false;

    public IEnumerable<Guid> FamilyIds =>
        httpContextAccessor.HttpContext?.User?
            .FindAll(DomainClaimTypes.FamilyMember)
            .Select(c => Guid.TryParse(c.Value, out var g) ? g : (Guid?)null)
            .Where(g => g.HasValue)
            .Select(g => g!.Value)
        ?? [];
}
