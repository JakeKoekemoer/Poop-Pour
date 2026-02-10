using MediatR;
using PoopNPour.Abstractions.Authentication;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.Authentication.Exceptions;
using PoopNPour.Application.Authentication.Models;
using PoopNPour.Application.Authorization;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.Authentication.Commands;

/// <summary>
/// Command to register a new user
/// </summary>
[Authorize(Policy = Policies.AccountRegistration)]
public record RegisterUserCommand(
    string Email,
    string UserName,
    string Password,
    string? FirstName = null,
    string? LastName = null) 
    : IRequest<RegisterResponseDto>;

/// <summary>
/// Handler for RegisterUserCommand
/// </summary>
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterResponseDto>
{
    private readonly IUserService _userService;
    private readonly IJwtTokenService _jwtTokenService;

    public RegisterUserCommandHandler(
        IUserService userService,
        IJwtTokenService jwtTokenService)
    {
        _userService = userService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<RegisterResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUserByEmail = await _userService.GetUserByEmailAsync(request.Email, cancellationToken);
        var existingUserByUserName = await _userService.GetUserByUserNameAsync(request.UserName, cancellationToken);

        if (existingUserByEmail != null || existingUserByUserName != null)
        {
            throw new UserAlreadyExistsException(request.Email);
        }

        var user = await _userService.CreateUserAsync(
            request.UserName,
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName,
            cancellationToken);

        (string token, DateTime expiresAt) = await _jwtTokenService.GenerateTokenAsync(
            user.Id,
            user.Email,
            user.UserName,
            user.Roles);

        return new RegisterResponseDto
        {
            User = user,
            Token = token,
            ExpiresAt = expiresAt
        };
    }
}
