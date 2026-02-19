using MediatR;
using PoopNPour.Abstractions.MedicineLog;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Common.Models;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.MedicineLogs.Queries.GetMedicineLogList;

/// <summary>
/// Query to get paginated list of medicine logs
/// </summary>
[Authorize(Policy = Policies.CanViewLogs)]
public record GetMedicineLogListQuery(
    int PageNumber = 1,
    int PageSize = 10,
    Guid? DependentId = null,
    string? MedicineName = null,
    DateTimeOffset? StartDate = null,
    DateTimeOffset? EndDate = null) 
    : IRequest<PaginatedResponseDto<MedicineLogDto>>;

/// <summary>
/// Handler for GetMedicineLogListQuery
/// </summary>
public class GetMedicineLogListQueryHandler(IMedicineLogService medicineLogService) : IRequestHandler<GetMedicineLogListQuery, PaginatedResponseDto<MedicineLogDto>>
{
    public async Task<PaginatedResponseDto<MedicineLogDto>> Handle(GetMedicineLogListQuery request, CancellationToken cancellationToken)
    {
        var (medicineLogs, totalCount) = await medicineLogService.GetMedicineLogsAsync(
            request.PageNumber,
            request.PageSize,
            request.DependentId,
            request.MedicineName,
            request.StartDate,
            request.EndDate,
            cancellationToken);

        return new PaginatedResponseDto<MedicineLogDto>
        {
            Items = medicineLogs,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}
