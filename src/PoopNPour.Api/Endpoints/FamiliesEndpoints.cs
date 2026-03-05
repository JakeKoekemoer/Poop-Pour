using MediatR;
using Microsoft.AspNetCore.Mvc;
using PoopNPour.Abstractions.Family;
using PoopNPour.Api.Common;
using PoopNPour.Application.Common.Models;
using PoopNPour.Application.Families.Commands.CreateFamily;
using PoopNPour.Application.Families.Commands.DeleteFamily;
using PoopNPour.Application.Families.Commands.UpdateFamily;
using PoopNPour.Application.Families.Queries.GetFamilyById;
using PoopNPour.Application.Families.Queries.GetFamilyList;

namespace PoopNPour.Api.Endpoints;

/// <summary>
/// Family management endpoints
/// Route: /api/families (auto-derived from class name)
/// </summary>
public class FamiliesEndpoints : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(CreateFamilyAsync, "", route => route
                .WithDocumentation("Create family", "Create a new family")
                .WithRequestResponse<CreateFamilyCommand, FamilyDto>())
            .MapPut(UpdateFamilyAsync, "{id:guid}", route => route
                .WithDocumentation("Update family", "Update an existing family")
                .WithRequestResponse<UpdateFamilyCommand, FamilyDto>())
            .MapDelete(DeleteFamilyAsync, "{id:guid}", route => route
                .WithDocumentation("Delete family", "Delete a family and all related data")
                .WithResponse<object>())
            .MapGet(GetFamilyByIdAsync, "{id:guid}", route => route
                .WithDocumentation("Get family", "Fetch a family by ID")
                .WithResponse<FamilyDto>())
            .MapGet(GetFamiliesAsync, "", route => route
                .WithDocumentation("List families", "Paginated list with optional search")
                .WithResponse<PaginatedResponseDto<FamilyDto>>());
    }

    public async Task<IResult> CreateFamilyAsync(
        IMediator mediator,
        [FromBody] CreateFamilyCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Results.Created($"/api/families/{result.FamilyId}", result);
    }

    public async Task<IResult> UpdateFamilyAsync(
        IMediator mediator,
        [FromRoute] Guid id,
        [FromBody] UpdateFamilyCommand command,
        CancellationToken cancellationToken)
    {
        // Ensure the ID in the route matches the command
        var updatedCommand = command with { FamilyId = id };
        var result = await mediator.Send(updatedCommand, cancellationToken);
        return Results.Ok(result);
    }

    public async Task<IResult> DeleteFamilyAsync(
        IMediator mediator,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteFamilyCommand(id);
        await mediator.Send(command, cancellationToken);
        return Results.NoContent();
    }

    public async Task<IResult> GetFamilyByIdAsync(
        IMediator mediator,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetFamilyByIdQuery(id);
        var result = await mediator.Send(query, cancellationToken);
        return Results.Ok(result);
    }

    public async Task<IResult> GetFamiliesAsync(
        IMediator mediator,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetFamilyListQuery(page, pageSize, searchTerm);
        var result = await mediator.Send(query, cancellationToken);
        return Results.Ok(result);
    }
}
