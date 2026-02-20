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
public class UpdateMyProfileCommandHandler(
    IUserService userService,
    IUser user) : IRequestHandler<UpdateMyProfileCommand, UserDto>
{
    public async Task<UserDto> Handle(UpdateMyProfileCommand request, CancellationToken cancellationToken)
    {
        return await userService.UpdateUserAsync(
            user.Id!,
            request.FirstName,
            request.LastName,
            request.Email,
            cancellationToken);
    }
}
