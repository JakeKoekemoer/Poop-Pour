using MediatR;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Users.Exceptions;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.Users.Commands;

/// <summary>
/// Command to remove claims from a user. Restricted to administrators.
/// Returns the remaining claim list after removal.
/// </summary>
[Authorize(Policy = Policies.CanManageUserClaims)]
public record RemoveUserClaimsCommand(string UserId, IEnumerable<ClaimDto> Claims)
    : IRequest<IList<ClaimDto>>;

/// <summary>
/// Handler for RemoveUserClaimsCommand
/// </summary>
public class RemoveUserClaimsCommandHandler(IUserService userService)
    : IRequestHandler<RemoveUserClaimsCommand, IList<ClaimDto>>
{
    public async Task<IList<ClaimDto>> Handle(RemoveUserClaimsCommand request, CancellationToken cancellationToken)
    {
        var user = await userService.GetUserByIdAsync(request.UserId, cancellationToken);

        if (user == null)
            throw new UserNotFoundException(request.UserId);

        return await userService.RemoveUserClaimsAsync(request.UserId, request.Claims, cancellationToken);
    }
}
