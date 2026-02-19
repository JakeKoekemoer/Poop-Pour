using MediatR;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Common.Models;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.FamilyUsers.Queries.GetFamilyUsersList;

/// <summary>
/// Query to get paginated list of family users
/// </summary>
[Authorize(Policy = Policies.CanViewFamilies)]
public record GetFamilyUsersListQuery(
    int PageNumber = 1,
    int PageSize = 10,
    Guid? FamilyId = null,
    string? UserId = null) 
    : IRequest<PaginatedResponseDto<FamilyUserDto>>;

/// <summary>
/// Handler for GetFamilyUsersListQuery
/// </summary>
public class GetFamilyUsersListQueryHandler(IFamilyUserService familyUserService) : IRequestHandler<GetFamilyUsersListQuery, PaginatedResponseDto<FamilyUserDto>>
{
    public async Task<PaginatedResponseDto<FamilyUserDto>> Handle(GetFamilyUsersListQuery request, CancellationToken cancellationToken)
    {
        var (familyUsers, totalCount) = await familyUserService.GetFamilyUsersAsync(
            request.PageNumber,
            request.PageSize,
            request.FamilyId,
            request.UserId,
            cancellationToken);

        return new PaginatedResponseDto<FamilyUserDto>
        {
            Items = familyUsers,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}
