using MediatR;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Users.Models;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.Users.Commands;

/// <summary>
/// Command to update current user's profile
/// </summary>
[Authorize(Policy = Policies.Can_ManageOwnProfile)]
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
    private readonly IUserService _userService;
    private readonly IUser _user;

    public UpdateMyProfileCommandHandler(
        IUserService userService,
        IUser user)
    {
        _userService = userService;
        _user = user;
    }

    public async Task<UserDto> Handle(UpdateMyProfileCommand request, CancellationToken cancellationToken)
    {
        return await _userService.UpdateUserAsync(
            _user.Id!,
            request.FirstName,
            request.LastName,
            request.Email,
            cancellationToken);
    }
}
