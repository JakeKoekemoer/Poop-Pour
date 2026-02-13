using FluentValidation;

namespace PoopNPour.Application.Settings.Commands.UpdateSystemSettings;

/// <summary>
/// Validator for UpdateSystemSettingsCommand
/// </summary>
public class UpdateSystemSettingsCommandValidator : AbstractValidator<UpdateSystemSettingsCommand>
{
    public UpdateSystemSettingsCommandValidator()
    {
        RuleFor(x => x.SetupCompleted)
            .NotNull().WithMessage("SetupCompleted is required.");
    }
}
