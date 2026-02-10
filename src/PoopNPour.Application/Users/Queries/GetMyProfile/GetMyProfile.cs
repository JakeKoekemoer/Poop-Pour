using AutoMapper;
using MediatR;
using PoopNPour.Application.Authorization;
using PoopNPour.Abstractions.Identity;
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
    private readonly IIdentityService _identityService;
    private readonly IUser _user;
    private readonly IMapper _mapper;

    public GetMyProfileQueryHandler(
        IIdentityService identityService,
        IUser user,
        IMapper mapper)
    {
        _identityService = identityService;
        _user = user;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
    {
        // Authorization behavior ensures user is authenticated and _user.Id is not null
        var user = await _identityService.GetUserByIdAsync(_user.Id!, cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundException(_user.Id!);
        }

        var roles = await _identityService.GetUserRolesAsync(user, cancellationToken);
        var userDto = _mapper.Map<UserDto>(user);
        userDto.Roles = roles;

        return userDto;
    }
}
