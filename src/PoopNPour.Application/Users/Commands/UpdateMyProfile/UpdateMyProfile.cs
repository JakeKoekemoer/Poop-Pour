using AutoMapper;
using MediatR;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Common.Interfaces;
using PoopNPour.Application.Identity;
using PoopNPour.Application.Users.Exceptions;
using PoopNPour.Application.Users.Models;

namespace PoopNPour.Application.Users.Commands;

/// <summary>
/// Command to update current user's profile
/// </summary>
[Authorize]  // Requires authentication
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
    private readonly IUser _user;
    private readonly IMapper _mapper;

    public UpdateMyProfileCommandHandler(
        IIdentityService identityService,
        IUser user,
        IMapper mapper)
    {
        _identityService = identityService;
        _user = user;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(UpdateMyProfileCommand request, CancellationToken cancellationToken)
    {
        // Authorization behavior ensures user is authenticated and _user.Id is not null
        var user = await _identityService.GetUserByIdAsync(_user.Id!, cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundException(_user.Id!);
        }

        // Check if email is being changed and if it's already in use
        if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
        {
            var existingUser = await _identityService.GetUserByEmailAsync(request.Email, cancellationToken);
            if (existingUser != null && existingUser.Id != _user.Id)
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
