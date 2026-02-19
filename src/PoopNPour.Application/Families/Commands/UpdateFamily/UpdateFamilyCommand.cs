using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using PoopNPour.Abstractions.Family;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Families.Exceptions;
using PoopNPour.Domain.Common.Auth;
using System.Security.Claims;

namespace PoopNPour.Application.Families.Commands.UpdateFamily;

/// <summary>
/// Command to update an existing family
/// </summary>
[Authorize(Policy = Policies.CanManageFamilies)]
public record UpdateFamilyCommand(
    Guid FamilyId,
    string? FamilyName = null,
    string? FamilyLastName = null) 
    : IRequest<FamilyDto>;

/// <summary>
/// Validator for UpdateFamilyCommand
/// </summary>
public class UpdateFamilyCommandValidator : AbstractValidator<UpdateFamilyCommand>
{
    public UpdateFamilyCommandValidator()
    {
        RuleFor(x => x.FamilyId)
            .NotEmpty()
            .WithMessage("Family ID is required.");

        When(x => x.FamilyName != null, () =>
        {
            RuleFor(x => x.FamilyName)
                .NotEmpty()
                .WithMessage("Family name cannot be empty if provided.")
                .MaximumLength(100)
                .WithMessage("Family name must not exceed 100 characters.");
        });

        When(x => x.FamilyLastName != null, () =>
        {
            RuleFor(x => x.FamilyLastName)
                .NotEmpty()
                .WithMessage("Family last name cannot be empty if provided.")
                .MaximumLength(100)
                .WithMessage("Family last name must not exceed 100 characters.");
        });
    }
}

/// <summary>
/// Handler for UpdateFamilyCommand
/// </summary>
public class UpdateFamilyCommandHandler(
    IFamilyService familyService,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<UpdateFamilyCommand, FamilyDto>
{
    public async Task<FamilyDto> Handle(UpdateFamilyCommand request, CancellationToken cancellationToken)
    {
        // Business validation: Check for duplicate family name if updating the name
        if (request.FamilyName != null)
        {
            var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                var isDuplicate = await familyService.IsFamilyNameDuplicateAsync(
                    request.FamilyName,
                    userId,
                    request.FamilyId,
                    cancellationToken);

                if (isDuplicate)
                {
                    throw new DuplicateFamilyNameException(request.FamilyName, userId);
                }
            }
        }

        var family = await familyService.UpdateFamilyAsync(
            request.FamilyId,
            request.FamilyName,
            request.FamilyLastName,
            cancellationToken);

        if (family == null)
        {
            throw new FamilyNotFoundException(request.FamilyId);
        }

        return family;
    }
}
