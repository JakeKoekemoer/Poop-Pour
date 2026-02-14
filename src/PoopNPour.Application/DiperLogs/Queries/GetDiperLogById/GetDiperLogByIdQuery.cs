using MediatR;
using PoopNPour.Abstractions.DiperLog;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.DiperLogs.Exceptions;
using PoopNPour.Application.DiperLogs.Models;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.DiperLogs.Queries.GetDiperLogById;

/// <summary>
/// Query to get a diper log by ID
/// </summary>
[Authorize(Policy = Policies.CanViewLogs)]
public record GetDiperLogByIdQuery(Guid DiperLogId) 
    : IRequest<DiperLogDto>;

/// <summary>
/// Handler for GetDiperLogByIdQuery
/// </summary>
public class GetDiperLogByIdQueryHandler(IDiperLogService diperLogService) : IRequestHandler<GetDiperLogByIdQuery, DiperLogDto>
{
    public async Task<DiperLogDto> Handle(GetDiperLogByIdQuery request, CancellationToken cancellationToken)
    {
        var diperLog = await diperLogService.GetDiperLogByIdAsync(request.DiperLogId, cancellationToken);

        if (diperLog == null)
        {
            throw new DiperLogNotFoundException(request.DiperLogId);
        }

        return diperLog;
    }
}
