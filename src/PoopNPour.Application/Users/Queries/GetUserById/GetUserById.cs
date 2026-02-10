using MediatR;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Users.Exceptions;
using PoopNPour.Application.Users.Models;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.Users.Queries;

/// <summary>
/// Query to get a user by ID
/// </summary>
[Authorize(Policies = new[] { Policies.CanViewUsers })]
public record GetUserByIdQuery(string UserId) 
    : IRequest<UserDto>;

/// <summary>
/// Handler for GetUserByIdQuery
/// </summary>
public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUserService _userService;

    public GetUserByIdQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByIdAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundException(request.UserId);
        }

        return user;
    }
}
