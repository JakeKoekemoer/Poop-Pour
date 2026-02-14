using FluentValidation;
using MediatR;
using PoopNPour.Abstractions.Dependent;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Dependents.Models;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.Dependents.Commands.CreateDependent;

/// <summary>
/// Command to create a new dependent
/// </summary>
[Authorize(Policy = Policies.CanManageDependents)]
public record CreateDependentCommand(
    Guid FamilyId,
    string DependentName,
    string DependentSurname,
    DateTimeOffset DateOfBirth) 
    : IRequest<DependentDto>;

/// <summary>
/// Validator for CreateDependentCommand
/// </summary>
public class CreateDependentCommandValidator : AbstractValidator<CreateDependentCommand>
{
    public CreateDependentCommandValidator()
    {
        RuleFor(x => x.FamilyId)
            .NotEmpty()
            .WithMessage("Family ID is required.");

        RuleFor(x => x.DependentName)
            .NotEmpty()
            .WithMessage("Dependent name is required.")
            .MaximumLength(100)
            .WithMessage("Dependent name must not exceed 100 characters.");

        RuleFor(x => x.DependentSurname)
            .NotEmpty()
            .WithMessage("Dependent surname is required.")
            .MaximumLength(100)
            .WithMessage("Dependent surname must not exceed 100 characters.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .WithMessage("Date of birth is required.");
    }
}

/// <summary>
/// Handler for CreateDependentCommand
/// </summary>
public class CreateDependentCommandHandler(IDependentService dependentService) : IRequestHandler<CreateDependentCommand, DependentDto>
{
    public async Task<DependentDto> Handle(CreateDependentCommand request, CancellationToken cancellationToken)
    {
        return await dependentService.CreateDependentAsync(
            request.FamilyId,
            request.DependentName,
            request.DependentSurname,
            request.DateOfBirth,
            cancellationToken);
    }
}
