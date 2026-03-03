using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PoopNPour.Abstractions.User;
using PoopNPour.Api.Common;
using PoopNPour.Application.Common.Models;
using PoopNPour.Application.Roles.Queries;
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
            // Queries (read)
            .MapGet(GetUsersAsync, "", route => route
                .WithDocumentation("List users", "Paginated list with optional search")
                .WithResponse<PaginatedResponseDto<UserDto>>())
            .MapGet(GetRolesAsync, "roles", route => route
                .WithDocumentation("List roles", "Get the list of all application roles (admin only)")
                .WithResponse<IReadOnlyList<string>>())
            .MapGet(GetUserByIdAsync, "{id}", route => route
                .WithDocumentation("Get user", "Fetch a user by ID")
                .WithResponse<UserDto>())
            .MapGet(GetMyProfileAsync, "me", route => route
                .WithDocumentation("My profile", "Get the current user's profile")
                .WithResponse<UserDto>())
            .MapGet(GetUserClaimsAsync, "{id}/claims", route => route
                .WithDocumentation("Get user claims", "Fetch all claims for a user (admin only)")
                .WithResponse<IList<ClaimDto>>())
            // Commands (write)
            .MapPost(CreateUserAsync, "", route => route
                .WithDocumentation("Create user", "Create a new user account (admin only)")
                .WithRequestResponse<CreateUserCommand, UserDto>(StatusCodes.Status201Created))
            .MapPut(UpdateMyProfileAsync, "me", route => route
                .WithDocumentation("Update my profile", "Update the current user's profile")
                .WithRequestResponse<UpdateProfileRequestDto, UserDto>())
            .MapPut(UpdateUserAsync, "{id}", route => route
                .WithDocumentation("Update user", "Update any user's details (admin only)")
                .WithRequestResponse<UpdateUserCommand, UserDto>())
            .MapDelete(DeleteUserAsync, "{id}", route => route
                .WithDocumentation("Delete user", "Permanently delete a user (admin only)")
                .Produces(StatusCodes.Status204NoContent))
            .MapPost(AddUserClaimsAsync, "{id}/claims", route => route
                .WithDocumentation("Add user claims", "Add claims to a user (admin only)")
                .WithRequestResponse<IEnumerable<ClaimDto>, IList<ClaimDto>>())
            .MapDelete(RemoveUserClaimsAsync, "{id}/claims", route => route
                .WithDocumentation("Remove user claims", "Remove claims from a user (admin only)")
                .WithRequestResponse<IEnumerable<ClaimDto>, IList<ClaimDto>>())
            .MapPost(AddUserRoleAsync, "{id}/roles", route => route
                .WithDocumentation("Add user role", "Add a role to a user (admin only)")
                .WithRequestResponse<AddUserRoleCommand, UserDto>())
            .MapDelete(RemoveUserRoleAsync, "{id}/roles/{role}", route => route
                .WithDocumentation("Remove user role", "Remove a role from a user (admin only)")
                .WithResponse<UserDto>());
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

    public async Task<IResult> GetRolesAsync(
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var roles = await mediator.Send(new GetRolesQuery(), cancellationToken);
        return Results.Ok(roles);
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

    public async Task<IResult> GetUserClaimsAsync(
        IMediator mediator,
        [FromRoute] string id,
        CancellationToken cancellationToken)
    {
        var query = new GetUserClaimsQuery(id);
        var result = await mediator.Send(query, cancellationToken);
        return Results.Ok(result);
    }

    public async Task<IResult> CreateUserAsync(
        IMediator mediator,
        [FromBody] CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Results.Created($"/api/users/{result.Id}", result);
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

    public async Task<IResult> UpdateUserAsync(
        IMediator mediator,
        [FromRoute] string id,
        [FromBody] UpdateUserCommand command,
        CancellationToken cancellationToken)
    {
        var resolvedCommand = command with { UserId = id };
        var result = await mediator.Send(resolvedCommand, cancellationToken);
        return Results.Ok(result);
    }

    public async Task<IResult> DeleteUserAsync(
        IMediator mediator,
        [FromRoute] string id,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteUserCommand(id), cancellationToken);
        return Results.NoContent();
    }

    public async Task<IResult> AddUserClaimsAsync(
        IMediator mediator,
        [FromRoute] string id,
        [FromBody] IEnumerable<ClaimDto> claims,
        CancellationToken cancellationToken)
    {
        var command = new AddUserClaimsCommand(id, claims);
        var result = await mediator.Send(command, cancellationToken);
        return Results.Ok(result);
    }

    public async Task<IResult> RemoveUserClaimsAsync(
        IMediator mediator,
        [FromRoute] string id,
        [FromBody] IEnumerable<ClaimDto> claims,
        CancellationToken cancellationToken)
    {
        var command = new RemoveUserClaimsCommand(id, claims);
        var result = await mediator.Send(command, cancellationToken);
        return Results.Ok(result);
    }

    public async Task<IResult> AddUserRoleAsync(
        IMediator mediator,
        [FromRoute] string id,
        [FromBody] AddUserRoleCommand command,
        CancellationToken cancellationToken)
    {
        var resolvedCommand = command with { UserId = id };
        var result = await mediator.Send(resolvedCommand, cancellationToken);
        return Results.Ok(result);
    }

    public async Task<IResult> RemoveUserRoleAsync(
        IMediator mediator,
        [FromRoute] string id,
        [FromRoute] string role,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RemoveUserRoleCommand(id, role), cancellationToken);
        return Results.Ok(result);
    }
}
