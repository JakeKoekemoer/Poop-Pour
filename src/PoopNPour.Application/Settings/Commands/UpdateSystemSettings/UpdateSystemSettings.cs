using MediatR;
using PoopNPour.Abstractions.Settings;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Common.Exceptions;
using PoopNPour.Domain.Common.Auth;
using PoopNPour.Domain.Models.Settings;

namespace PoopNPour.Application.Settings.Commands.UpdateSystemSettings;

[Authorize(Policy = Policies.CanManageSystemSettings)]
public record UpdateSystemSettingsCommand(
    bool? SetupCompleted
) : IRequest;

public class UpdateSystemSettingsCommandHandler(
    ISettingsRepository settingsRepository) : IRequestHandler<UpdateSystemSettingsCommand>
{
    public async Task Handle(UpdateSystemSettingsCommand request, CancellationToken cancellationToken)
    {
        if (!request.SetupCompleted.HasValue)
        {
            throw new ValidationException("SetupCompleted", "SetupCompleted is required.");
        }

        var systemSettings = settingsRepository.LoadSetting<SystemSettings>();

        settingsRepository.UpdateBooleanSetting(
            request.SetupCompleted,
            systemSettings.SetupCompleted,
            value => systemSettings.SetupCompleted = value,
            "SetupCompleted");

        await settingsRepository.SaveSettingsAsync(systemSettings, cancellationToken: cancellationToken);
    }
}
