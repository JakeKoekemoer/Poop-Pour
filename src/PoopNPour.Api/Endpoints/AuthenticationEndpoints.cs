using MediatR;
using Microsoft.AspNetCore.Mvc;
using PoopNPour.Api.Common;
using PoopNPour.Application.Authentication.Commands;
using PoopNPour.Application.Authentication.Models;

namespace PoopNPour.Api.Endpoints;

/// <summary>
/// Authentication endpoints
/// Route: /api/authentication (auto-derived from class name)
/// </summary>
public class AuthenticationEndpoints : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(LoginAsync, "login", route => route
                .WithSummary("Authenticate user and get JWT token")
            )
            .MapPost(RegisterAsync, "register", route => route
                .WithSummary("Register a new user")
            );
    }

    public async Task<IResult> LoginAsync(
        IMediator mediator,
        [FromBody] LoginRequestDto request,
        CancellationToken cancellationToken)
    {
        var command = new AuthenticateUserCommand(request.Username, request.Password);
        var result = await mediator.Send(command, cancellationToken);
        return Results.Ok(result);
    }

    public async Task<IResult> RegisterAsync(
        IMediator mediator,
        [FromBody] RegisterRequestDto request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.UserName,
            request.Password,
            request.FirstName,
            request.LastName);

        var result = await mediator.Send(command, cancellationToken);
        return Results.Created($"/api/users/{result.User.Id}", result);
    }
}
