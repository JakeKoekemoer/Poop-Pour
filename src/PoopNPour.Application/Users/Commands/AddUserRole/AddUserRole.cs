using MediatR;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Users.Exceptions;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.Users.Commands;

/// <summary>
/// Command to add a role to a user. Restricted to administrators.
/// Returns the updated UserDto reflecting the new role.
/// </summary>
[Authorize(Policy = Policies.CanManageUsers)]
public record AddUserRoleCommand(string UserId, string Role) : IRequest<UserDto>;

/// <summary>
/// Handler for AddUserRoleCommand
/// </summary>
public class AddUserRoleCommandHandler(IUserService userService)
    : IRequestHandler<AddUserRoleCommand, UserDto>
{
    public async Task<UserDto> Handle(AddUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await userService.GetUserByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            throw new UserNotFoundException(request.UserId);

        return await userService.AddUserRoleAsync(request.UserId, request.Role, cancellationToken);
    }
}
