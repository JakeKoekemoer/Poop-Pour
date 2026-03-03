using FluentValidation;

namespace PoopNPour.Application.Users.Commands;

/// <summary>
/// Validator for AddUserRoleCommand
/// </summary>
public class AddUserRoleCommandValidator : AbstractValidator<AddUserRoleCommand>
{
    private static readonly IReadOnlySet<string> ValidRoles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        PoopNPour.Domain.Common.Auth.Roles.Administrator,
        PoopNPour.Domain.Common.Auth.Roles.Tenant,
        PoopNPour.Domain.Common.Auth.Roles.Web_Api,
        PoopNPour.Domain.Common.Auth.Roles.Mobile_Api
    };

    public AddUserRoleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role is required.")
            .Must(r => ValidRoles.Contains(r))
            .WithMessage($"Role must be one of: {string.Join(", ", ValidRoles)}.");
    }
}
