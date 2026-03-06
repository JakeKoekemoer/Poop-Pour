using MediatR;
using Microsoft.AspNetCore.Mvc;
using PoopNPour.Abstractions.DiperLog;
using PoopNPour.Api.Common;
using PoopNPour.Application.Common.Models;
using PoopNPour.Application.DiperLogs.Commands.CreateDiperLog;
using PoopNPour.Application.DiperLogs.Commands.UpdateDiperLog;
using PoopNPour.Application.DiperLogs.Queries.GetDiperLogById;
using PoopNPour.Application.DiperLogs.Queries.GetDiperLogList;

namespace PoopNPour.Api.Endpoints;

/// <summary>
/// Diaper log management endpoints
/// Route: /api/diper-logs (auto-derived from class name)
/// </summary>
public class DiperLogsEndpoints : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(CreateDiperLogAsync, "", route => route
                .WithDocumentation("Create diaper log", "Record a new diaper change")
                .WithRequestResponse<CreateDiperLogCommand, DiperLogDto>())
            .MapPut(UpdateDiperLogAsync, "{id:guid}", route => route
                .WithDocumentation("Update diaper log", "Update an existing diaper log")
                .WithRequestResponse<UpdateDiperLogCommand, DiperLogDto>())
            .MapGet(GetDiperLogByIdAsync, "{id:guid}", route => route
                .WithDocumentation("Get diaper log", "Fetch a diaper log by ID")
                .WithResponse<DiperLogDto>())
            .MapGet(GetDiperLogsAsync, "", route => route
                .WithDocumentation("List diaper logs", "Paginated list with optional filters")
                .WithResponse<PaginatedResponseDto<DiperLogDto>>());
    }

    public async Task<IResult> CreateDiperLogAsync(
        IMediator mediator,
        [FromBody] CreateDiperLogCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Results.Created($"/api/diper-logs/{result.DiperLogId}", result);
    }

    public async Task<IResult> UpdateDiperLogAsync(
        IMediator mediator,
        [FromRoute] Guid id,
        [FromBody] UpdateDiperLogCommand command,
        CancellationToken cancellationToken)
    {
        var updatedCommand = command with { DiperLogId = id };
        var result = await mediator.Send(updatedCommand, cancellationToken);
        return Results.Ok(result);
    }

    public async Task<IResult> GetDiperLogByIdAsync(
        IMediator mediator,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetDiperLogByIdQuery(id);
        var result = await mediator.Send(query, cancellationToken);
        return Results.Ok(result);
    }

    public async Task<IResult> GetDiperLogsAsync(
        IMediator mediator,
        HttpContext httpContext,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? dependentId = null,
        [FromQuery] DateTimeOffset? startDate = null,
        [FromQuery] DateTimeOffset? endDate = null,
        CancellationToken cancellationToken = default)
    {
        Guid? familyId = httpContext.Request.Headers.TryGetValue("X-Family-Id", out var h)
            && Guid.TryParse(h, out var g) ? g : null;

        var query = new GetDiperLogListQuery(page, pageSize, dependentId, startDate, endDate, familyId);
        var result = await mediator.Send(query, cancellationToken);
        return Results.Ok(result);
    }
}
