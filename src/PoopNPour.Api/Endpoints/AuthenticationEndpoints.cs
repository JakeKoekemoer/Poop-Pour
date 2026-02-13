using MediatR;
using Microsoft.AspNetCore.Http;
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
            .RequireAuthorization()
            .MapPost(LoginAsync, "login", route => route
                .WithDocumentation("Log in", "Returns a JWT token for authenticated requests")
                .WithRequestResponse<LoginRequestDto, LoginResponseDto>())
            .MapPost(RegisterAsync, "register", route => route
                .WithDocumentation("Register", "Create a new user account")
                .WithRequestResponse<RegisterRequestDto, RegisterResponseDto>(StatusCodes.Status201Created));
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
