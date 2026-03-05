using MediatR;
using Microsoft.AspNetCore.Mvc;
using PoopNPour.Abstractions.Dependent;
using PoopNPour.Api.Common;
using PoopNPour.Application.Common.Models;
using PoopNPour.Application.Dependents.Commands.CreateDependent;
using PoopNPour.Application.Dependents.Commands.DeleteDependent;
using PoopNPour.Application.Dependents.Commands.UpdateDependent;
using PoopNPour.Application.Dependents.Queries.GetDependentById;
using PoopNPour.Application.Dependents.Queries.GetDependentList;

namespace PoopNPour.Api.Endpoints;

/// <summary>
/// Dependent management endpoints
/// Route: /api/dependents (auto-derived from class name)
/// </summary>
public class DependentsEndpoints : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(CreateDependentAsync, "", route => route
                .WithDocumentation("Create dependent", "Create a new dependent")
                .WithRequestResponse<CreateDependentCommand, DependentDto>())
            .MapPut(UpdateDependentAsync, "{id:guid}", route => route
                .WithDocumentation("Update dependent", "Update an existing dependent")
                .WithRequestResponse<UpdateDependentCommand, DependentDto>())
            .MapDelete(DeleteDependentAsync, "{id:guid}", route => route
                .WithDocumentation("Delete dependent", "Delete a dependent and all related data")
                .WithResponse<object>())
            .MapGet(GetDependentByIdAsync, "{id:guid}", route => route
                .WithDocumentation("Get dependent", "Fetch a dependent by ID")
                .WithResponse<DependentDto>())
            .MapGet(GetDependentsAsync, "", route => route
                .WithDocumentation("List dependents", "Paginated list with optional filters")
                .WithResponse<PaginatedResponseDto<DependentDto>>());
    }

    public async Task<IResult> CreateDependentAsync(
        IMediator mediator,
        [FromBody] CreateDependentCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Results.Created($"/api/dependents/{result.DependentId}", result);
    }

    public async Task<IResult> UpdateDependentAsync(
        IMediator mediator,
        [FromRoute] Guid id,
        [FromBody] UpdateDependentCommand command,
        CancellationToken cancellationToken)
    {
        var updatedCommand = command with { DependentId = id };
        var result = await mediator.Send(updatedCommand, cancellationToken);
        return Results.Ok(result);
    }

    public async Task<IResult> DeleteDependentAsync(
        IMediator mediator,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteDependentCommand(id);
        await mediator.Send(command, cancellationToken);
        return Results.NoContent();
    }

    public async Task<IResult> GetDependentByIdAsync(
        IMediator mediator,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetDependentByIdQuery(id);
        var result = await mediator.Send(query, cancellationToken);
        return Results.Ok(result);
    }

    public async Task<IResult> GetDependentsAsync(
        IMediator mediator,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? familyId = null,
        [FromQuery] string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetDependentListQuery(page, pageSize, familyId, searchTerm);
        var result = await mediator.Send(query, cancellationToken);
        return Results.Ok(result);
    }
}
