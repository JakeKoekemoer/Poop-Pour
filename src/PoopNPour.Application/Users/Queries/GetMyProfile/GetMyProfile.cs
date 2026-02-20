using MediatR;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Users.Exceptions;
using PoopNPour.Application.Users.Models;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.Users.Queries;

/// <summary>
/// Query to get current user's profile
/// </summary>
[Authorize(Policy = Policies.Can_ManageOwnProfile)]
public record GetMyProfileQuery() 
    : IRequest<UserDto>;

/// <summary>
/// Handler for GetMyProfileQuery
/// </summary>
public class GetMyProfileQueryHandler(
    IUserService userService,
    IUser currentUser) : IRequestHandler<GetMyProfileQuery, UserDto>
{
    public async Task<UserDto> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await userService.GetUserByIdAsync(currentUser.Id!, cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundException(currentUser.Id!);
        }

        return user;
    }
}
