using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using PoopNPour.Application.Identity;
using PoopNPour.Application.Users.Exceptions;
using PoopNPour.Application.Users.Models;
using System.Security.Claims;

namespace PoopNPour.Application.Users.Queries;

/// <summary>
/// Query to get current user's profile
/// </summary>
public record GetMyProfileQuery() 
    : IRequest<UserDto>;

/// <summary>
/// Handler for GetMyProfileQuery
/// </summary>
public class GetMyProfileQueryHandler : IRequestHandler<GetMyProfileQuery, UserDto>
{
    private readonly IIdentityService _identityService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public GetMyProfileQueryHandler(
        IIdentityService identityService,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper)
    {
        _identityService = identityService;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var user = await _identityService.GetUserByIdAsync(userId, cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }

        var roles = await _identityService.GetUserRolesAsync(user, cancellationToken);
        var userDto = _mapper.Map<UserDto>(user);
        userDto.Roles = roles;

        return userDto;
    }
}
