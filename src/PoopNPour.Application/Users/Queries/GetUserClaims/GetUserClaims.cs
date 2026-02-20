using MediatR;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Users.Exceptions;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.Users.Queries;

/// <summary>
/// Query to get all claims for a user by ID. Restricted to administrators.
/// </summary>
[Authorize(Policy = Policies.CanViewUserClaims)]
public record GetUserClaimsQuery(string UserId) : IRequest<IList<ClaimDto>>;

/// <summary>
/// Handler for GetUserClaimsQuery
/// </summary>
public class GetUserClaimsQueryHandler(IUserService userService)
    : IRequestHandler<GetUserClaimsQuery, IList<ClaimDto>>
{
    public async Task<IList<ClaimDto>> Handle(GetUserClaimsQuery request, CancellationToken cancellationToken)
    {
        var user = await userService.GetUserByIdAsync(request.UserId, cancellationToken);

        if (user == null)
            throw new UserNotFoundException(request.UserId);

        return await userService.GetUserClaimsAsync(request.UserId, cancellationToken);
    }
}
