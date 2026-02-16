using MediatR;
using Microsoft.AspNetCore.Mvc;
using PoopNPour.Abstractions.FeedLog;
using PoopNPour.Api.Common;
using PoopNPour.Application.Common.Models;
using PoopNPour.Application.FeedLogs.Commands.CreateFeedLog;
using PoopNPour.Application.FeedLogs.Commands.UpdateFeedLog;
using PoopNPour.Application.FeedLogs.Queries.GetFeedLogById;
using PoopNPour.Application.FeedLogs.Queries.GetFeedLogList;

namespace PoopNPour.Api.Endpoints;

/// <summary>
/// Feed log management endpoints
/// Route: /api/feed-logs (auto-derived from class name)
/// </summary>
public class FeedLogsEndpoints : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(CreateFeedLogAsync, "", route => route
                .WithDocumentation("Create feed log", "Record a new feeding")
                .WithRequestResponse<CreateFeedLogCommand, FeedLogDto>())
            .MapPut(UpdateFeedLogAsync, "{id:guid}", route => route
                .WithDocumentation("Update feed log", "Update an existing feed log")
                .WithRequestResponse<UpdateFeedLogCommand, FeedLogDto>())
            .MapGet(GetFeedLogByIdAsync, "{id:guid}", route => route
                .WithDocumentation("Get feed log", "Fetch a feed log by ID")
                .WithResponse<FeedLogDto>())
            .MapGet(GetFeedLogsAsync, "", route => route
                .WithDocumentation("List feed logs", "Paginated list with optional filters")
                .WithResponse<PaginatedResponseDto<FeedLogDto>>());
    }

    public async Task<IResult> CreateFeedLogAsync(
        IMediator mediator,
        [FromBody] CreateFeedLogCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Results.Created($"/api/feed-logs/{result.FeedLogId}", result);
    }

    public async Task<IResult> UpdateFeedLogAsync(
        IMediator mediator,
        [FromRoute] Guid id,
        [FromBody] UpdateFeedLogCommand command,
        CancellationToken cancellationToken)
    {
        var updatedCommand = command with { FeedLogId = id };
        var result = await mediator.Send(updatedCommand, cancellationToken);
        return Results.Ok(result);
    }

    public async Task<IResult> GetFeedLogByIdAsync(
        IMediator mediator,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetFeedLogByIdQuery(id);
        var result = await mediator.Send(query, cancellationToken);
        return Results.Ok(result);
    }

    public async Task<IResult> GetFeedLogsAsync(
        IMediator mediator,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? dependentId = null,
        [FromQuery] DateTimeOffset? startDate = null,
        [FromQuery] DateTimeOffset? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetFeedLogListQuery(page, pageSize, dependentId, startDate, endDate);
        var result = await mediator.Send(query, cancellationToken);
        return Results.Ok(result);
    }
}
