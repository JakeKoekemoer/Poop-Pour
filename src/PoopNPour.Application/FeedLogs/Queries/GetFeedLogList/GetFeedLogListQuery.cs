using MediatR;
using PoopNPour.Abstractions.FeedLog;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Common.Models;
using PoopNPour.Application.FeedLogs.Models;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.FeedLogs.Queries.GetFeedLogList;

/// <summary>
/// Query to get paginated list of feed logs
/// </summary>
[Authorize(Policy = Policies.CanViewLogs)]
public record GetFeedLogListQuery(
    int PageNumber = 1,
    int PageSize = 10,
    Guid? DependentId = null,
    DateTimeOffset? StartDate = null,
    DateTimeOffset? EndDate = null) 
    : IRequest<PaginatedResponseDto<FeedLogDto>>;

/// <summary>
/// Handler for GetFeedLogListQuery
/// </summary>
public class GetFeedLogListQueryHandler(IFeedLogService feedLogService) : IRequestHandler<GetFeedLogListQuery, PaginatedResponseDto<FeedLogDto>>
{
    public async Task<PaginatedResponseDto<FeedLogDto>> Handle(GetFeedLogListQuery request, CancellationToken cancellationToken)
    {
        var (feedLogs, totalCount) = await feedLogService.GetFeedLogsAsync(
            request.PageNumber,
            request.PageSize,
            request.DependentId,
            request.StartDate,
            request.EndDate,
            cancellationToken);

        return new PaginatedResponseDto<FeedLogDto>
        {
            Items = feedLogs,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}
