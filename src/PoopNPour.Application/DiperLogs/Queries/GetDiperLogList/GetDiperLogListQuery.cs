using MediatR;
using PoopNPour.Abstractions.DiperLog;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Common.Models;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.DiperLogs.Queries.GetDiperLogList;

/// <summary>
/// Query to get paginated list of diper logs
/// </summary>
[Authorize(Policy = Policies.CanViewLogs)]
public record GetDiperLogListQuery(
    int PageNumber = 1,
    int PageSize = 10,
    Guid? DependentId = null,
    DateTimeOffset? StartDate = null,
    DateTimeOffset? EndDate = null) 
    : IRequest<PaginatedResponseDto<DiperLogDto>>;

/// <summary>
/// Handler for GetDiperLogListQuery
/// </summary>
public class GetDiperLogListQueryHandler(IDiperLogService diperLogService) : IRequestHandler<GetDiperLogListQuery, PaginatedResponseDto<DiperLogDto>>
{
    public async Task<PaginatedResponseDto<DiperLogDto>> Handle(GetDiperLogListQuery request, CancellationToken cancellationToken)
    {
        var (diperLogs, totalCount) = await diperLogService.GetDiperLogsAsync(
            request.PageNumber,
            request.PageSize,
            request.DependentId,
            request.StartDate,
            request.EndDate,
            cancellationToken);

        return new PaginatedResponseDto<DiperLogDto>
        {
            Items = diperLogs,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}
