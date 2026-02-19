using MediatR;
using PoopNPour.Abstractions.Family;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Common.Models;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.Families.Queries.GetFamilyList;

/// <summary>
/// Query to get paginated list of families
/// </summary>
[Authorize(Policy = Policies.CanViewFamilies)]
public record GetFamilyListQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null) 
    : IRequest<PaginatedResponseDto<FamilyDto>>;

/// <summary>
/// Handler for GetFamilyListQuery
/// </summary>
public class GetFamilyListQueryHandler(IFamilyService familyService) : IRequestHandler<GetFamilyListQuery, PaginatedResponseDto<FamilyDto>>
{
    public async Task<PaginatedResponseDto<FamilyDto>> Handle(GetFamilyListQuery request, CancellationToken cancellationToken)
    {
        var (families, totalCount) = await familyService.GetFamiliesAsync(
            request.PageNumber,
            request.PageSize,
            request.SearchTerm,
            cancellationToken);

        return new PaginatedResponseDto<FamilyDto>
        {
            Items = families,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}
