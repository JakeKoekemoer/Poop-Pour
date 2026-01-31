using AutoMapper;
using MediatR;
using PoopNPour.Application.Authentication;
using PoopNPour.Application.Authentication.Exceptions;
using PoopNPour.Application.Authentication.Models;
using PoopNPour.Application.Identity;
using PoopNPour.Application.Users.Models;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.Authentication.Commands;

/// <summary>
/// Command to register a new user
/// </summary>
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
    private readonly IIdentityService _identityService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(
        IIdentityService identityService,
        IJwtTokenService jwtTokenService,
        IMapper mapper)
    {
        _identityService = identityService;
        _jwtTokenService = jwtTokenService;
        _mapper = mapper;
    }

    public async Task<RegisterResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // Check if user already exists
        var existingUserByEmail = await _identityService.GetUserByEmailAsync(request.Email, cancellationToken);
        var existingUserByUserName = await _identityService.GetUserByUserNameAsync(request.UserName, cancellationToken);

        if (existingUserByEmail != null || existingUserByUserName != null)
        {
            throw new UserAlreadyExistsException(request.Email);
        }

        // Create user
        var user = await _identityService.CreateUserAsync(
            request.UserName,
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName,
            cancellationToken);

        // Assign default role (WebApi or MobileApi - for now using WebApi)
        await _identityService.AddUserToRoleAsync(user, Roles.WebApi, cancellationToken);

        // Get user roles
        var roles = await _identityService.GetUserRolesAsync(user, cancellationToken);

        // Generate JWT token
        (string token, DateTime expiresAt) = await _jwtTokenService.GenerateTokenAsync(user, roles);

        // Map user to DTO
        var userDto = _mapper.Map<UserDto>(user);
        userDto.Roles = roles;

        return new RegisterResponseDto
        {
            User = userDto,
            Token = token,
            ExpiresAt = expiresAt
        };
    }
}
