using AutoMapper;
using MediatR;
using PoopNPour.Application.Authorization;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Application.Users.Models;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.Users.Queries;

/// <summary>
/// Query to get paginated list of users
/// </summary>
[Authorize(Policies = new[] { Policies.CanViewUsers })]
public record GetUsersQuery(int PageNumber, int PageSize, string? SearchTerm = null) 
    : IRequest<PaginatedResponseDto<UserDto>>;

/// <summary>
/// Handler for GetUsersQuery
/// </summary>
public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PaginatedResponseDto<UserDto>>
{
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public GetUsersQueryHandler(
        IIdentityService identityService,
        IMapper mapper)
    {
        _identityService = identityService;
        _mapper = mapper;
    }

    public async Task<PaginatedResponseDto<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var (users, totalCount) = await _identityService.GetUsersAsync(
            request.PageNumber,
            request.PageSize,
            request.SearchTerm,
            cancellationToken);

        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await _identityService.GetUserRolesAsync(user, cancellationToken);
            var userDto = _mapper.Map<UserDto>(user);
            userDto.Roles = roles;
            userDtos.Add(userDto);
        }

        return new PaginatedResponseDto<UserDto>
        {
            Items = userDtos,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}
