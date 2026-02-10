using AutoMapper;
using MediatR;
using PoopNPour.Application.Authorization;
using PoopNPour.Abstractions.Identity;
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
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(
        IIdentityService identityService,
        IMapper mapper)
    {
        _identityService = identityService;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _identityService.GetUserByIdAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundException(request.UserId);
        }

        var roles = await _identityService.GetUserRolesAsync(user, cancellationToken);
        var userDto = _mapper.Map<UserDto>(user);
        userDto.Roles = roles;

        return userDto;
    }
}
