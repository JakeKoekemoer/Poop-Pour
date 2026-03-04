using MediatR;
using Microsoft.AspNetCore.Mvc;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Api.Common;
using PoopNPour.Application.Common.Models;
using PoopNPour.Application.FamilyUsers.Commands.AddUserToFamily;
using PoopNPour.Application.FamilyUsers.Commands.RemoveUserFromFamily;
using PoopNPour.Application.FamilyUsers.Queries.GetFamilyMember;
using PoopNPour.Application.FamilyUsers.Queries.GetFamilyMembers;
using PoopNPour.Application.FamilyUsers.Queries.GetFamilyUserById;
using PoopNPour.Application.FamilyUsers.Queries.GetFamilyUsersList;

namespace PoopNPour.Api.Endpoints;

/// <summary>
/// Family user management endpoints
/// Route: /api/family-users (auto-derived from class name)
/// </summary>
public class FamilyUsersEndpoints : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(AddUserToFamilyAsync, "", route => route
                .WithDocumentation("Add user to family", "Add a user to a family")
                .WithRequestResponse<AddUserToFamilyCommand, FamilyUserDto>())
            .MapDelete(RemoveUserFromFamilyAsync, "{familyId:guid}/users/{userId}", route => route
                .WithDocumentation("Remove user from family", "Remove a user from a family")
                .WithResponse<object>())
            .MapGet(GetFamilyUserByIdAsync, "{familyId:guid}/users/{userId}", route => route
                .WithDocumentation("Get family user", "Fetch a family user by composite key")
                .WithResponse<FamilyUserDto>())
            .MapGet(GetFamilyUsersAsync, "", route => route
                .WithDocumentation("List family users", "Paginated list with optional filters")
                .WithResponse<PaginatedResponseDto<FamilyUserDto>>())
            .MapGet(GetFamilyMembersAsync, "{familyId:guid}/members", route => route
                .WithDocumentation("List family members", "Paginated list of enriched members (includes user name, email, family name)")
                .WithResponse<PaginatedResponseDto<FamilyMemberDto>>())
            .MapGet(GetFamilyMemberAsync, "{familyId:guid}/members/{userId}", route => route
                .WithDocumentation("Get family member", "Fetch a single enriched family member by userId")
                .WithResponse<FamilyMemberDto>());
    }

    public async Task<IResult> AddUserToFamilyAsync(
        IMediator mediator,
        [FromBody] AddUserToFamilyCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Results.Created($"/api/family-users/{result.FamilyId}/users/{result.UserId}", result);
    }

    public async Task<IResult> RemoveUserFromFamilyAsync(
        IMediator mediator,
        [FromRoute] Guid familyId,
        [FromRoute] string userId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveUserFromFamilyCommand(familyId, userId);
        await mediator.Send(command, cancellationToken);
        return Results.NoContent();
    }

    public async Task<IResult> GetFamilyUserByIdAsync(
        IMediator mediator,
        [FromRoute] Guid familyId,
        [FromRoute] string userId,
        CancellationToken cancellationToken)
    {
        var query = new GetFamilyUserByIdQuery(familyId, userId);
        var result = await mediator.Send(query, cancellationToken);
        return Results.Ok(result);
    }

    public async Task<IResult> GetFamilyUsersAsync(
        IMediator mediator,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? familyId = null,
        [FromQuery] string? userId = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetFamilyUsersListQuery(page, pageSize, familyId, userId);
        var result = await mediator.Send(query, cancellationToken);
        return Results.Ok(result);
    }

    public async Task<IResult> GetFamilyMembersAsync(
        IMediator mediator,
        [FromRoute] Guid familyId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new GetFamilyMembersQuery(familyId, page, pageSize);
        var result = await mediator.Send(query, cancellationToken);
        return Results.Ok(result);
    }

    public async Task<IResult> GetFamilyMemberAsync(
        IMediator mediator,
        [FromRoute] Guid familyId,
        [FromRoute] string userId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetFamilyMemberQuery(familyId, userId);
        var result = await mediator.Send(query, cancellationToken);
        return Results.Ok(result);
    }
}
