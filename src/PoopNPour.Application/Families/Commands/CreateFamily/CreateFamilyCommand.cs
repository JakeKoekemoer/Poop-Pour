using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using PoopNPour.Abstractions.Family;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Families.Exceptions;
using PoopNPour.Domain.Common.Auth;
using System.Security.Claims;

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
            .MaximumLength(100)
            .WithMessage("Family name must not exceed 100 characters.");

        RuleFor(x => x.FamilyLastName)
            .NotEmpty()
            .WithMessage("Family last name is required.")
            .MaximumLength(100)
            .WithMessage("Family last name must not exceed 100 characters.");
    }
}

/// <summary>
/// Handler for CreateFamilyCommand
/// </summary>
public class CreateFamilyCommandHandler(
    IFamilyService familyService,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<CreateFamilyCommand, FamilyDto>
{
    public async Task<FamilyDto> Handle(CreateFamilyCommand request, CancellationToken cancellationToken)
    {
        // Business validation: Check for duplicate family name
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (!string.IsNullOrEmpty(userId))
        {
            var isDuplicate = await familyService.IsFamilyNameDuplicateAsync(
                request.FamilyName,
                userId,
                null,
                cancellationToken);

            if (isDuplicate)
            {
                throw new DuplicateFamilyNameException(request.FamilyName, userId);
            }
        }

        return await familyService.CreateFamilyAsync(
            request.FamilyName,
            request.FamilyLastName,
            cancellationToken);
    }
}
