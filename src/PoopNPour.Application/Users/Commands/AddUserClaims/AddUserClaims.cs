using MediatR;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Users.Exceptions;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.Users.Commands;

/// <summary>
/// Command to add claims to a user. Restricted to administrators.
/// Returns the full updated claim list after adding.
/// </summary>
[Authorize(Policy = Policies.CanManageUserClaims)]
public record AddUserClaimsCommand(string UserId, IEnumerable<ClaimDto> Claims)
    : IRequest<IList<ClaimDto>>;

/// <summary>
/// Handler for AddUserClaimsCommand
/// </summary>
public class AddUserClaimsCommandHandler(IUserService userService)
    : IRequestHandler<AddUserClaimsCommand, IList<ClaimDto>>
{
    public async Task<IList<ClaimDto>> Handle(AddUserClaimsCommand request, CancellationToken cancellationToken)
    {
        var user = await userService.GetUserByIdAsync(request.UserId, cancellationToken);

        if (user == null)
            throw new UserNotFoundException(request.UserId);

        return await userService.AddUserClaimsAsync(request.UserId, request.Claims, cancellationToken);
    }
}
