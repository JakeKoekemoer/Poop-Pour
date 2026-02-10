using AutoMapper;
using MediatR;
using PoopNPour.Abstractions.Authentication;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Application.Authentication.Exceptions;
using PoopNPour.Application.Authentication.Models;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Users.Models;
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
public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, LoginResponseDto>
{
    private readonly IIdentityService _identityService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IMapper _mapper;

    public AuthenticateUserCommandHandler(
        IIdentityService identityService,
        IJwtTokenService jwtTokenService,
        IMapper mapper)
    {
        _identityService = identityService;
        _jwtTokenService = jwtTokenService;
        _mapper = mapper;
    }

    public async Task<LoginResponseDto> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        // Validate password
        var user = await _identityService.ValidatePasswordAsync(
            request.Username, 
            request.Password, 
            cancellationToken);

        if (user == null)
        {
            throw new InvalidCredentialsException();
        }

        // Get user roles
        var roles = await _identityService.GetUserRolesAsync(user, cancellationToken);

        // Generate JWT token
        (string token, DateTime expiresAt) = await _jwtTokenService.GenerateTokenAsync(user, roles);

        // Map user to DTO
        var userDto = _mapper.Map<UserDto>(user);
        userDto.Roles = roles;

        return new LoginResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt,
            User = userDto
        };
    }
}
