using MediatR;
using Microsoft.AspNetCore.Mvc;
using PoopNPour.Abstractions.MedicineLog;
using PoopNPour.Api.Common;
using PoopNPour.Application.Common.Models;
using PoopNPour.Application.MedicineLogs.Commands.CreateMedicineLog;
using PoopNPour.Application.MedicineLogs.Commands.UpdateMedicineLog;
using PoopNPour.Application.MedicineLogs.Queries.GetMedicineLogById;
using PoopNPour.Application.MedicineLogs.Queries.GetMedicineLogList;

namespace PoopNPour.Api.Endpoints;

/// <summary>
/// Medicine log management endpoints
/// Route: /api/medicine-logs (auto-derived from class name)
/// </summary>
public class MedicineLogsEndpoints : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(CreateMedicineLogAsync, "", route => route
                .WithDocumentation("Create medicine log", "Record a new medicine administration")
                .WithRequestResponse<CreateMedicineLogCommand, MedicineLogDto>())
            .MapPut(UpdateMedicineLogAsync, "{id:guid}", route => route
                .WithDocumentation("Update medicine log", "Update an existing medicine log")
                .WithRequestResponse<UpdateMedicineLogCommand, MedicineLogDto>())
            .MapGet(GetMedicineLogByIdAsync, "{id:guid}", route => route
                .WithDocumentation("Get medicine log", "Fetch a medicine log by ID")
                .WithResponse<MedicineLogDto>())
            .MapGet(GetMedicineLogsAsync, "", route => route
                .WithDocumentation("List medicine logs", "Paginated list with optional filters")
                .WithResponse<PaginatedResponseDto<MedicineLogDto>>());
    }

    public async Task<IResult> CreateMedicineLogAsync(
        IMediator mediator,
        [FromBody] CreateMedicineLogCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Results.Created($"/api/medicine-logs/{result.MedicineLogId}", result);
    }

    public async Task<IResult> UpdateMedicineLogAsync(
        IMediator mediator,
        [FromRoute] Guid id,
        [FromBody] UpdateMedicineLogCommand command,
        CancellationToken cancellationToken)
    {
        var updatedCommand = command with { MedicineLogId = id };
        var result = await mediator.Send(updatedCommand, cancellationToken);
        return Results.Ok(result);
    }

    public async Task<IResult> GetMedicineLogByIdAsync(
        IMediator mediator,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetMedicineLogByIdQuery(id);
        var result = await mediator.Send(query, cancellationToken);
        return Results.Ok(result);
    }

    public async Task<IResult> GetMedicineLogsAsync(
        IMediator mediator,
        HttpContext httpContext,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? dependentId = null,
        [FromQuery] string? medicineName = null,
        [FromQuery] DateTimeOffset? startDate = null,
        [FromQuery] DateTimeOffset? endDate = null,
        CancellationToken cancellationToken = default)
    {
        Guid? familyId = httpContext.Request.Headers.TryGetValue("X-Family-Id", out var h)
            && Guid.TryParse(h, out var g) ? g : null;

        var query = new GetMedicineLogListQuery(page, pageSize, dependentId, medicineName, startDate, endDate, familyId);
        var result = await mediator.Send(query, cancellationToken);
        return Results.Ok(result);
    }
}
