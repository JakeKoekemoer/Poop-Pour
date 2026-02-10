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
public class GetMyProfileQueryHandler : IRequestHandler<GetMyProfileQuery, UserDto>
{
    private readonly IUserService _userService;
    private readonly IUser _user;

    public GetMyProfileQueryHandler(
        IUserService userService,
        IUser user)
    {
        _userService = userService;
        _user = user;
    }

    public async Task<UserDto> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByIdAsync(_user.Id!, cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundException(_user.Id!);
        }

        return user;
    }
}
