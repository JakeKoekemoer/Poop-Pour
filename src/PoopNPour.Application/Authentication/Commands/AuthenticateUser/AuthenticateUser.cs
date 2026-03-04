using MediatR;
using PoopNPour.Abstractions.Authentication;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.Authentication.Exceptions;
using PoopNPour.Application.Authentication.Models;
using PoopNPour.Application.Authorization;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.Authentication.Commands;

/// <summary>
/// Command to authenticate a user and return JWT token
/// </summary>
[Authorize(Policy = Policies.Can_AuthenticateUser)]
public record AuthenticateUserCommand(string Username, string Password) 
    : IRequest<LoginResponseDto>;

/// <summary>
/// Handler for AuthenticateUserCommand
/// </summary>
public class AuthenticateUserCommandHandler(
    IUserService userService,
    IJwtTokenService jwtTokenService,
    IFamilyUserService familyUserService) : IRequestHandler<AuthenticateUserCommand, LoginResponseDto>
{
    public async Task<LoginResponseDto> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userService.ValidatePasswordAndGetUserAsync(
            request.Username,
            request.Password,
            cancellationToken);

        if (user == null)
        {
            throw new InvalidCredentialsException();
        }

        var familyMemberships = await familyUserService.GetUserFamilyMembershipsAsync(
            user.Id,
            cancellationToken);

        (string token, DateTime expiresAt) = await jwtTokenService.GenerateTokenAsync(
            user.Id,
            user.Email,
            user.UserName,
            user.Roles,
            familyMemberships);

        return new LoginResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt,
            User = user
        };
    }
}
