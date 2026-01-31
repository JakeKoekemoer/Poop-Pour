using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using PoopNPour.Application.Identity;
using PoopNPour.Application.Users.Exceptions;
using PoopNPour.Application.Users.Models;
using System.Security.Claims;

namespace PoopNPour.Application.Users.Commands;

/// <summary>
/// Command to update current user's profile
/// </summary>
public record UpdateMyProfileCommand(
    string? FirstName = null,
    string? LastName = null,
    string? Email = null) 
    : IRequest<UserDto>;

/// <summary>
/// Handler for UpdateMyProfileCommand
/// </summary>
public class UpdateMyProfileCommandHandler : IRequestHandler<UpdateMyProfileCommand, UserDto>
{
    private readonly IIdentityService _identityService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public UpdateMyProfileCommandHandler(
        IIdentityService identityService,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper)
    {
        _identityService = identityService;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(UpdateMyProfileCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var user = await _identityService.GetUserByIdAsync(userId, cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }

        // Check if email is being changed and if it's already in use
        if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
        {
            var existingUser = await _identityService.GetUserByEmailAsync(request.Email, cancellationToken);
            if (existingUser != null && existingUser.Id != userId)
            {
                throw new EmailAlreadyInUseException(request.Email);
            }
            user.Email = request.Email;
            user.NormalizedEmail = request.Email.ToUpperInvariant();
        }

        // Update user properties
        if (!string.IsNullOrEmpty(request.FirstName))
        {
            user.FirstName = request.FirstName;
        }

        if (!string.IsNullOrEmpty(request.LastName))
        {
            user.LastName = request.LastName;
        }

        user.UpdatedAt = DateTime.UtcNow;

        // Save changes
        var updatedUser = await _identityService.UpdateUserAsync(user, cancellationToken);

        // Get user roles
        var roles = await _identityService.GetUserRolesAsync(updatedUser, cancellationToken);

        // Map to DTO
        var userDto = _mapper.Map<UserDto>(updatedUser);
        userDto.Roles = roles;

        return userDto;
    }
}
