using FluentValidation;
using MediatR;
using PoopNPour.Abstractions.Family;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Families.Models;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.Families.Commands.CreateFamily;

/// <summary>
/// Command to create a new family
/// </summary>
[Authorize(Policy = Policies.CanManageFamilies)]
public record CreateFamilyCommand(
    string FamilyName,
    string FamilyLastName) 
    : IRequest<FamilyDto>;

/// <summary>
/// Validator for CreateFamilyCommand
/// </summary>
public class CreateFamilyCommandValidator : AbstractValidator<CreateFamilyCommand>
{
    public CreateFamilyCommandValidator()
    {
        RuleFor(x => x.FamilyName)
            .NotEmpty()
            .WithMessage("Family name is required.")
            .MaximumLength(200)
            .WithMessage("Family name must not exceed 200 characters.");

        RuleFor(x => x.FamilyLastName)
            .NotEmpty()
            .WithMessage("Family last name is required.")
            .MaximumLength(200)
            .WithMessage("Family last name must not exceed 200 characters.");
    }
}

/// <summary>
/// Handler for CreateFamilyCommand
/// </summary>
public class CreateFamilyCommandHandler(IFamilyService familyService) : IRequestHandler<CreateFamilyCommand, FamilyDto>
{
    public async Task<FamilyDto> Handle(CreateFamilyCommand request, CancellationToken cancellationToken)
    {
        return await familyService.CreateFamilyAsync(
            request.FamilyName,
            request.FamilyLastName,
            cancellationToken);
    }
}
