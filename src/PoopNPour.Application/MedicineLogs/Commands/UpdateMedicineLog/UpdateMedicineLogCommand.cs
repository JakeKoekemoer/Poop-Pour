using FluentValidation;
using MediatR;
using PoopNPour.Abstractions.MedicineLog;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.MedicineLogs.Exceptions;
using PoopNPour.Application.MedicineLogs.Models;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.MedicineLogs.Commands.UpdateMedicineLog;

/// <summary>
/// Command to update an existing medicine log
/// </summary>
[Authorize(Policy = Policies.CanManageLogs)]
public record UpdateMedicineLogCommand(
    Guid MedicineLogId,
    string? MedicineName = null,
    string? Dosage = null,
    DateTimeOffset? TimeAdministered = null,
    ICollection<string>? Notes = null) 
    : IRequest<MedicineLogDto>;

/// <summary>
/// Validator for UpdateMedicineLogCommand
/// </summary>
public class UpdateMedicineLogCommandValidator : AbstractValidator<UpdateMedicineLogCommand>
{
    public UpdateMedicineLogCommandValidator()
    {
        RuleFor(x => x.MedicineLogId)
            .NotEmpty()
            .WithMessage("Medicine log ID is required.");

        When(x => x.MedicineName != null, () =>
        {
            RuleFor(x => x.MedicineName)
                .NotEmpty()
                .WithMessage("Medicine name cannot be empty if provided.")
                .MaximumLength(200)
                .WithMessage("Medicine name must not exceed 200 characters.");
        });

        When(x => x.Dosage != null, () =>
        {
            RuleFor(x => x.Dosage)
                .NotEmpty()
                .WithMessage("Dosage cannot be empty if provided.")
                .MaximumLength(100)
                .WithMessage("Dosage must not exceed 100 characters.");
        });
    }
}

/// <summary>
/// Handler for UpdateMedicineLogCommand
/// </summary>
public class UpdateMedicineLogCommandHandler(IMedicineLogService medicineLogService) : IRequestHandler<UpdateMedicineLogCommand, MedicineLogDto>
{
    public async Task<MedicineLogDto> Handle(UpdateMedicineLogCommand request, CancellationToken cancellationToken)
    {
        var medicineLog = await medicineLogService.UpdateMedicineLogAsync(
            request.MedicineLogId,
            request.MedicineName,
            request.Dosage,
            request.TimeAdministered,
            request.Notes,
            cancellationToken);

        if (medicineLog == null)
        {
            throw new MedicineLogNotFoundException(request.MedicineLogId);
        }

        return medicineLog;
    }
}
