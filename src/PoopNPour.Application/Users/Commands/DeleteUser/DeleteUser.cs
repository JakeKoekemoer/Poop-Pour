using MediatR;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Users.Exceptions;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.Users.Commands;

/// <summary>
/// Command to delete a user by ID. Restricted to administrators.
/// </summary>
[Authorize(Policy = Policies.CanManageUsers)]
public record DeleteUserCommand(string UserId) : IRequest<Unit>;

/// <summary>
/// Handler for DeleteUserCommand
/// </summary>
public class DeleteUserCommandHandler(IUserService userService)
    : IRequestHandler<DeleteUserCommand, Unit>
{
    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userService.GetUserByIdAsync(request.UserId, cancellationToken);

        if (user == null)
            throw new UserNotFoundException(request.UserId);

        await userService.DeleteUserAsync(request.UserId, cancellationToken);

        return Unit.Value;
    }
}
