using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoopNPour.Application.Users.Commands;
using PoopNPour.Application.Users.Exceptions;
using PoopNPour.Application.Users.Models;
using PoopNPour.Application.Users.Queries;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Api.Endpoints;

/// <summary>
/// User management endpoints
/// </summary>
public static class UsersEndpoints
{
    public static void MapUsersEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/users").WithTags("Users");

        group.MapGet("/", GetUsersAsync)
            .WithName("GetUsers")
            .WithSummary("Get paginated list of users")
            .RequireAuthorization(Policies.CanViewUsers)
            .Produces<PaginatedResponseDto<UserDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status403Forbidden);

        group.MapGet("/{id}", GetUserByIdAsync)
            .WithName("GetUserById")
            .WithSummary("Get user by ID")
            .RequireAuthorization(Policies.CanViewUsers)
            .Produces<UserDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status403Forbidden);

        group.MapGet("/me", GetMyProfileAsync)
            .WithName("GetMyProfile")
            .WithSummary("Get current user's profile")
            .RequireAuthorization()
            .Produces<UserDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);

        group.MapPut("/me", UpdateMyProfileAsync)
            .WithName("UpdateMyProfile")
            .WithSummary("Update current user's profile")
            .RequireAuthorization()
            .Accepts<UpdateProfileRequestDto>("application/json")
            .Produces<UserDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    private static async Task<IResult> GetUsersAsync(
        [FromServices] IMediator mediator,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetUsersQuery(page, pageSize, searchTerm);
        var result = await mediator.Send(query, cancellationToken);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetUserByIdAsync(
        [FromRoute] string id,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetUserByIdQuery(id);
            var result = await mediator.Send(query, cancellationToken);
            return Results.Ok(result);
        }
        catch (UserNotFoundException)
        {
            return Results.NotFound();
        }
    }

    private static async Task<IResult> GetMyProfileAsync(
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetMyProfileQuery();
            var result = await mediator.Send(query, cancellationToken);
            return Results.Ok(result);
        }
        catch (UserNotFoundException)
        {
            return Results.NotFound();
        }
    }

    private static async Task<IResult> UpdateMyProfileAsync(
        [FromBody] UpdateProfileRequestDto request,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new UpdateMyProfileCommand(
                request.FirstName,
                request.LastName,
                request.Email);

            var result = await mediator.Send(command, cancellationToken);
            return Results.Ok(result);
        }
        catch (UserNotFoundException)
        {
            return Results.NotFound();
        }
        catch (EmailAlreadyInUseException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }
}
