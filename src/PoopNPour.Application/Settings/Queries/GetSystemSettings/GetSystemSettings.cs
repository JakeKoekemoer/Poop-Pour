using MediatR;
using PoopNPour.Abstractions.Settings;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Settings.Models;
using PoopNPour.Domain.Common.Auth;
using PoopNPour.Domain.Models.Settings;

namespace PoopNPour.Application.Settings.Queries.GetSystemSettings;

[Authorize(Policy = Policies.CanManageSystemSettings)]
public record GetSystemSettingsQuery() : IRequest<SystemSettingsDto>;

public class GetSystemSettingsQueryHandler(
    ISettingsRepository settingsRepository) : IRequestHandler<GetSystemSettingsQuery, SystemSettingsDto>
{
    public Task<SystemSettingsDto> Handle(GetSystemSettingsQuery request, CancellationToken cancellationToken)
    {
        var settings = settingsRepository.LoadSetting<SystemSettings>();
        
        var dto = new SystemSettingsDto
        {
            SetupCompleted = settings.SetupCompleted
        };

        return Task.FromResult(dto);
    }
}
