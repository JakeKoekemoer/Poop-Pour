using MediatR;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Users.Exceptions;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.Users.Commands;

/// <summary>
/// Command to update any user's profile. Restricted to administrators.
/// </summary>
[Authorize(Policy = Policies.CanManageUsers)]
public record UpdateUserCommand(
    string UserId,
    string? FirstName = null,
    string? LastName = null,
    string? Email = null)
    : IRequest<UserDto>;

/// <summary>
/// Handler for UpdateUserCommand
/// </summary>
public class UpdateUserCommandHandler(IUserService userService)
    : IRequestHandler<UpdateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userService.GetUserByIdAsync(request.UserId, cancellationToken);

        if (user == null)
            throw new UserNotFoundException(request.UserId);

        return await userService.UpdateUserAsync(
            request.UserId,
            request.FirstName,
            request.LastName,
            request.Email,
            cancellationToken);
    }
}
