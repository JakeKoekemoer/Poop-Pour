using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.Authorization;

/// <summary>
/// Enforces family-scoped role authorization on a MediatR request.
/// The request must implement IFamilyRequest to supply the FamilyId.
/// Works in conjunction with FamilyAuthorizationBehavior.
/// </summary>
/// <example>
/// <code>
/// // Require at least Admin role within the family
/// [AuthorizeFamilyMember(FamilyRole.Admin)]
/// public record CreateDependentCommand(...) : IRequest&lt;DependentDto&gt;, IFamilyRequest;
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class AuthorizeFamilyMemberAttribute(FamilyRole minimumRole = FamilyRole.Member) : Attribute
{
    public FamilyRole MinimumRole { get; } = minimumRole;
}
