using MediatR;
using Microsoft.AspNetCore.Mvc;
using PoopNPour.Api.Common;
using PoopNPour.Application.Users.Commands;
using PoopNPour.Application.Users.Models;
using PoopNPour.Application.Users.Queries;

namespace PoopNPour.Api.Endpoints;

/// <summary>
/// User management endpoints
/// Route: /api/users (auto-derived from class name)
/// </summary>
public class UsersEndpoints : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetUsersAsync, "", route => route
                .WithDocumentation("List users", "Paginated list with optional search"))
            .MapGet(GetUserByIdAsync, "{id}", route => route
                .WithDocumentation("Get user", "Fetch a user by ID"))
            .MapGet(GetMyProfileAsync, "me", route => route
                .WithDocumentation("My profile", "Get the current user's profile"))
            .MapPut(UpdateMyProfileAsync, "me", route => route
                .WithDocumentation("Update my profile", "Update the current user's profile")
                .Accepts<UpdateProfileRequestDto>("application/json"));
    }

    public async Task<IResult> GetUsersAsync(
        IMediator mediator,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetUsersQuery(page, pageSize, searchTerm);
        var result = await mediator.Send(query, cancellationToken);
        return Results.Ok(result);
    }

    public async Task<IResult> GetUserByIdAsync(
        IMediator mediator,
        [FromRoute] string id,
        CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var result = await mediator.Send(query, cancellationToken);
        return Results.Ok(result);
    }

    public async Task<IResult> GetMyProfileAsync(
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new GetMyProfileQuery();
        var result = await mediator.Send(query, cancellationToken);
        return Results.Ok(result);
    }

    public async Task<IResult> UpdateMyProfileAsync(
        IMediator mediator,
        [FromBody] UpdateProfileRequestDto request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateMyProfileCommand(
            request.FirstName,
            request.LastName,
            request.Email);

        var result = await mediator.Send(command, cancellationToken);
        return Results.Ok(result);
    }
}
