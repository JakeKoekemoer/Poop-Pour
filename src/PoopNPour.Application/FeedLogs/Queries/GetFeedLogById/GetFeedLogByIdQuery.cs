using MediatR;
using PoopNPour.Abstractions.FeedLog;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.FeedLogs.Exceptions;
using PoopNPour.Application.FeedLogs.Models;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.FeedLogs.Queries.GetFeedLogById;

/// <summary>
/// Query to get a feed log by ID
/// </summary>
[Authorize(Policy = Policies.CanViewLogs)]
public record GetFeedLogByIdQuery(Guid FeedLogId) 
    : IRequest<FeedLogDto>;

/// <summary>
/// Handler for GetFeedLogByIdQuery
/// </summary>
public class GetFeedLogByIdQueryHandler(IFeedLogService feedLogService) : IRequestHandler<GetFeedLogByIdQuery, FeedLogDto>
{
    public async Task<FeedLogDto> Handle(GetFeedLogByIdQuery request, CancellationToken cancellationToken)
    {
        var feedLog = await feedLogService.GetFeedLogByIdAsync(request.FeedLogId, cancellationToken);

        if (feedLog == null)
        {
            throw new FeedLogNotFoundException(request.FeedLogId);
        }

        return feedLog;
    }
}
