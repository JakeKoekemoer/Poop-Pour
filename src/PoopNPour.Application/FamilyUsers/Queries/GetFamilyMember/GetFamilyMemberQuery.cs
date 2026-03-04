using MediatR;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Common.Interfaces;
using PoopNPour.Application.FamilyUsers.Exceptions;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.FamilyUsers.Queries.GetFamilyMember;

/// <summary>
/// Returns a single enriched family member record.
/// Requires the caller to be at least a Viewer in that family.
/// </summary>
[Authorize(Policy = Policies.CanViewFamilies)]
[AuthorizeFamilyMember(FamilyRole.Viewer)]
public record GetFamilyMemberQuery(
    Guid FamilyId,
    string UserId)
    : IRequest<FamilyMemberDto>, IFamilyRequest;

/// <summary>
/// Handler for GetFamilyMemberQuery
/// </summary>
public class GetFamilyMemberQueryHandler(IFamilyUserService familyUserService)
    : IRequestHandler<GetFamilyMemberQuery, FamilyMemberDto>
{
    public async Task<FamilyMemberDto> Handle(
        GetFamilyMemberQuery request,
        CancellationToken cancellationToken)
    {
        var member = await familyUserService.GetFamilyMemberAsync(
            request.FamilyId,
            request.UserId,
            cancellationToken);

        if (member is null)
            throw new FamilyUserNotFoundException(request.FamilyId, request.UserId);

        return member;
    }
}
