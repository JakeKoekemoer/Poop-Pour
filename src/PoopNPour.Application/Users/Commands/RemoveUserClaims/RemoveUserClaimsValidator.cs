using FluentValidation;

namespace PoopNPour.Application.Users.Commands;

/// <summary>
/// Validator for RemoveUserClaimsCommand
/// </summary>
public class RemoveUserClaimsCommandValidator : AbstractValidator<RemoveUserClaimsCommand>
{
    public RemoveUserClaimsCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.Claims)
            .NotNull().WithMessage("Claims list is required.")
            .Must(c => c.Any()).WithMessage("At least one claim must be provided.");

        RuleForEach(x => x.Claims)
            .ChildRules(claim =>
            {
                claim.RuleFor(c => c.Type)
                    .NotEmpty().WithMessage("Claim type is required.");

                claim.RuleFor(c => c.Value)
                    .NotEmpty().WithMessage("Claim value is required.");
            });
    }
}
