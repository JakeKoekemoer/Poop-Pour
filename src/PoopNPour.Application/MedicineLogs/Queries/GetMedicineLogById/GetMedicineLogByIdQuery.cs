using MediatR;
using PoopNPour.Abstractions.MedicineLog;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.MedicineLogs.Exceptions;
using PoopNPour.Application.MedicineLogs.Models;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.MedicineLogs.Queries.GetMedicineLogById;

/// <summary>
/// Query to get a medicine log by ID
/// </summary>
[Authorize(Policy = Policies.CanViewLogs)]
public record GetMedicineLogByIdQuery(Guid MedicineLogId) 
    : IRequest<MedicineLogDto>;

/// <summary>
/// Handler for GetMedicineLogByIdQuery
/// </summary>
public class GetMedicineLogByIdQueryHandler(IMedicineLogService medicineLogService) : IRequestHandler<GetMedicineLogByIdQuery, MedicineLogDto>
{
    public async Task<MedicineLogDto> Handle(GetMedicineLogByIdQuery request, CancellationToken cancellationToken)
    {
        var medicineLog = await medicineLogService.GetMedicineLogByIdAsync(request.MedicineLogId, cancellationToken);

        if (medicineLog == null)
        {
            throw new MedicineLogNotFoundException(request.MedicineLogId);
        }

        return medicineLog;
    }
}
