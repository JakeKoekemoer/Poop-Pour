using MediatR;
using Microsoft.AspNetCore.Mvc;
using PoopNPour.Api.Common;
using PoopNPour.Application.Settings.Commands.UpdateSystemSettings;
using PoopNPour.Application.Settings.Queries.GetSystemSettings;

namespace PoopNPour.Api.Endpoints;

/// <summary>
/// Settings management endpoints
/// Route: /api/settings (auto-derived from class name)
/// </summary>
public class SettingsEndpoints : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetSystemSettings, "system", route => route
                .WithDocumentation("Get system settings", "Retrieve current system settings"))
            .MapPost(UpdateSystemSettings, "system", route => route
                .WithDocumentation("Update system settings", "Update system settings")
                .Accepts<UpdateSystemSettingsCommand>("application/json"));
    }

    public async Task<IResult> GetSystemSettings(
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new GetSystemSettingsQuery();
        var result = await mediator.Send(query, cancellationToken);
        return Results.Ok(result);
    }

    public async Task<IResult> UpdateSystemSettings(
        IMediator mediator,
        [FromBody] UpdateSystemSettingsCommand command,
        CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return Results.Ok();
    }
}
