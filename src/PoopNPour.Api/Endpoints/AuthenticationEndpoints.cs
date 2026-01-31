using MediatR;
using Microsoft.AspNetCore.Mvc;
using PoopNPour.Application.Authentication.Commands;
using PoopNPour.Application.Authentication.Exceptions;
using PoopNPour.Application.Authentication.Models;

namespace PoopNPour.Api.Endpoints;

/// <summary>
/// Authentication endpoints
/// </summary>
public static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/auth").WithTags("Authentication");

        group.MapPost("/login", LoginAsync)
            .WithName("Login")
            .WithSummary("Authenticate user and get JWT token")
            .Accepts<LoginRequestDto>("application/json")
            .Produces<LoginResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        group.MapPost("/register", RegisterAsync)
            .WithName("Register")
            .WithSummary("Register a new user")
            .Accepts<RegisterRequestDto>("application/json")
            .Produces<RegisterResponseDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> LoginAsync(
        [FromBody] LoginRequestDto request,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new AuthenticateUserCommand(request.EmailOrUserName, request.Password);
            var result = await mediator.Send(command, cancellationToken);
            return Results.Ok(result);
        }
        catch (InvalidCredentialsException)
        {
            return Results.Unauthorized();
        }
    }

    private static async Task<IResult> RegisterAsync(
        [FromBody] RegisterRequestDto request,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        try
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
        catch (UserAlreadyExistsException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }
}
