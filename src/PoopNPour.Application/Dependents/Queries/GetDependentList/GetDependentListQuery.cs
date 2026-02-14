using MediatR;
using PoopNPour.Abstractions.Dependent;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Common.Models;
using PoopNPour.Application.Dependents.Models;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.Dependents.Queries.GetDependentList;

/// <summary>
/// Query to get paginated list of dependents
/// </summary>
[Authorize(Policy = Policies.CanViewDependents)]
public record GetDependentListQuery(
    int PageNumber = 1,
    int PageSize = 10,
    Guid? FamilyId = null,
    string? SearchTerm = null) 
    : IRequest<PaginatedResponseDto<DependentDto>>;

/// <summary>
/// Handler for GetDependentListQuery
/// </summary>
public class GetDependentListQueryHandler(IDependentService dependentService) : IRequestHandler<GetDependentListQuery, PaginatedResponseDto<DependentDto>>
{
    public async Task<PaginatedResponseDto<DependentDto>> Handle(GetDependentListQuery request, CancellationToken cancellationToken)
    {
        var (dependents, totalCount) = await dependentService.GetDependentsAsync(
            request.PageNumber,
            request.PageSize,
            request.FamilyId,
            request.SearchTerm,
            cancellationToken);

        return new PaginatedResponseDto<DependentDto>
        {
            Items = dependents,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}
