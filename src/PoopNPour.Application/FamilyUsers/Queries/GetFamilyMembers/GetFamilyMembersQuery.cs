using MediatR;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Common.Interfaces;
using PoopNPour.Application.Common.Models;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.FamilyUsers.Queries.GetFamilyMembers;

/// <summary>
/// Returns a paginated, enriched list of members for a given family.
/// Requires the caller to be at least a Viewer in that family (enforced by FamilyAuthorizationBehavior).
/// </summary>
[Authorize(Policy = Policies.CanViewFamilies)]
[AuthorizeFamilyMember(FamilyRole.Viewer)]
public record GetFamilyMembersQuery(
    Guid FamilyId,
    int PageNumber = 1,
    int PageSize = 20)
    : IRequest<PaginatedResponseDto<FamilyMemberDto>>, IFamilyRequest;

/// <summary>
/// Handler for GetFamilyMembersQuery
/// </summary>
public class GetFamilyMembersQueryHandler(IFamilyUserService familyUserService)
    : IRequestHandler<GetFamilyMembersQuery, PaginatedResponseDto<FamilyMemberDto>>
{
    public async Task<PaginatedResponseDto<FamilyMemberDto>> Handle(
        GetFamilyMembersQuery request,
        CancellationToken cancellationToken)
    {
        var (members, totalCount) = await familyUserService.GetFamilyMembersAsync(
            request.FamilyId,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        return new PaginatedResponseDto<FamilyMemberDto>
        {
            Items = members,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}
