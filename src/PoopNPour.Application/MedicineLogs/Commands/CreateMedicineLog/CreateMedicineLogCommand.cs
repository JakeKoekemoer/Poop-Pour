using FluentValidation;
using MediatR;
using PoopNPour.Abstractions.MedicineLog;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.MedicineLogs.Exceptions;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.MedicineLogs.Commands.CreateMedicineLog;

/// <summary>
/// Command to create a new medicine log
/// </summary>
[Authorize(Policy = Policies.CanManageLogs)]
public record CreateMedicineLogCommand(
    Guid DependentId,
    string MedicineName,
    string Dosage,
    DateTimeOffset TimeAdministered,
    ICollection<string>? Notes = null) 
    : IRequest<MedicineLogDto>;

/// <summary>
/// Validator for CreateMedicineLogCommand
/// </summary>
public class CreateMedicineLogCommandValidator : AbstractValidator<CreateMedicineLogCommand>
{
    public CreateMedicineLogCommandValidator()
    {
        RuleFor(x => x.DependentId)
            .NotEmpty()
            .WithMessage("Dependent ID is required.");

        RuleFor(x => x.MedicineName)
            .NotEmpty()
            .WithMessage("Medicine name is required.")
            .MaximumLength(200)
            .WithMessage("Medicine name must not exceed 200 characters.");

        RuleFor(x => x.Dosage)
            .NotEmpty()
            .WithMessage("Dosage is required.")
            .MaximumLength(100)
            .WithMessage("Dosage must not exceed 100 characters.");

        RuleFor(x => x.TimeAdministered)
            .NotEmpty()
            .WithMessage("Time administered is required.");
    }
}

/// <summary>
/// Handler for CreateMedicineLogCommand
/// </summary>
public class CreateMedicineLogCommandHandler(IMedicineLogService medicineLogService) : IRequestHandler<CreateMedicineLogCommand, MedicineLogDto>
{
    public async Task<MedicineLogDto> Handle(CreateMedicineLogCommand request, CancellationToken cancellationToken)
    {
        // Business validation: Check for duplicate medicine + timestamp
        var isDuplicate = await medicineLogService.IsMedicineLogDuplicateAsync(
            request.DependentId,
            request.MedicineName,
            request.TimeAdministered,
            null,
            cancellationToken);

        if (isDuplicate)
        {
            throw new DuplicateMedicineLogException(request.DependentId, request.MedicineName, request.TimeAdministered);
        }

        return await medicineLogService.CreateMedicineLogAsync(
            request.DependentId,
            request.MedicineName,
            request.Dosage,
            request.TimeAdministered,
            request.Notes,
            cancellationToken);
    }
}
