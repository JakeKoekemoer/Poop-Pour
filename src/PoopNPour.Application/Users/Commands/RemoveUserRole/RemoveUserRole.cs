using MediatR;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Users.Exceptions;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.Users.Commands;

/// <summary>
/// Command to remove a role from a user. Restricted to administrators.
/// Returns the updated UserDto reflecting the removed role.
/// </summary>
[Authorize(Policy = Policies.CanManageUsers)]
public record RemoveUserRoleCommand(string UserId, string Role) : IRequest<UserDto>;

/// <summary>
/// Handler for RemoveUserRoleCommand
/// </summary>
public class RemoveUserRoleCommandHandler(IUserService userService)
    : IRequestHandler<RemoveUserRoleCommand, UserDto>
{
    public async Task<UserDto> Handle(RemoveUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await userService.GetUserByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            throw new UserNotFoundException(request.UserId);

        return await userService.RemoveUserRoleAsync(request.UserId, request.Role, cancellationToken);
    }
}
