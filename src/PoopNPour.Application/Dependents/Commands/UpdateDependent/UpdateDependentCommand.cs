using FluentValidation;
using MediatR;
using PoopNPour.Abstractions.Dependent;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Dependents.Exceptions;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.Dependents.Commands.UpdateDependent;

/// <summary>
/// Command to update an existing dependent
/// </summary>
[Authorize(Policy = Policies.CanManageDependents)]
public record UpdateDependentCommand(
    Guid DependentId,
    string? DependentName = null,
    string? DependentSurname = null,
    DateTimeOffset? DateOfBirth = null) 
    : IRequest<DependentDto>;

/// <summary>
/// Validator for UpdateDependentCommand
/// </summary>
public class UpdateDependentCommandValidator : AbstractValidator<UpdateDependentCommand>
{
    public UpdateDependentCommandValidator()
    {
        RuleFor(x => x.DependentId)
            .NotEmpty()
            .WithMessage("Dependent ID is required.");

        When(x => x.DependentName != null, () =>
        {
            RuleFor(x => x.DependentName)
                .NotEmpty()
                .WithMessage("Dependent name cannot be empty if provided.")
                .MaximumLength(100)
                .WithMessage("Dependent name must not exceed 100 characters.");
        });

        When(x => x.DependentSurname != null, () =>
        {
            RuleFor(x => x.DependentSurname)
                .NotEmpty()
                .WithMessage("Dependent surname cannot be empty if provided.")
                .MaximumLength(100)
                .WithMessage("Dependent surname must not exceed 100 characters.");
        });
    }
}

/// <summary>
/// Handler for UpdateDependentCommand
/// </summary>
public class UpdateDependentCommandHandler(IDependentService dependentService) : IRequestHandler<UpdateDependentCommand, DependentDto>
{
    public async Task<DependentDto> Handle(UpdateDependentCommand request, CancellationToken cancellationToken)
    {
        // Business validation: Check for duplicate dependent name if updating name fields
        if (request.DependentName != null || request.DependentSurname != null)
        {
            var currentDependent = await dependentService.GetDependentByIdAsync(request.DependentId, cancellationToken);
            if (currentDependent != null)
            {
                var nameToCheck = request.DependentName ?? currentDependent.DependentName;
                var surnameToCheck = request.DependentSurname ?? currentDependent.DependentSurname;
                
                var isDuplicate = await dependentService.IsDependentNameDuplicateAsync(
                    currentDependent.FamilyId,
                    nameToCheck,
                    surnameToCheck,
                    request.DependentId,
                    cancellationToken);

                if (isDuplicate)
                {
                    throw new DuplicateDependentNameException(nameToCheck, surnameToCheck, currentDependent.FamilyId);
                }
            }
        }

        var dependent = await dependentService.UpdateDependentAsync(
            request.DependentId,
            request.DependentName,
            request.DependentSurname,
            request.DateOfBirth,
            cancellationToken);

        if (dependent == null)
        {
            throw new DependentNotFoundException(request.DependentId);
        }

        return dependent;
    }
}
