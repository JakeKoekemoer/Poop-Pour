using MediatR;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.Authorization;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.Users.Commands;

/// <summary>
/// Command to create a new user. Restricted to administrators.
/// </summary>
[Authorize(Policy = Policies.CanManageUsers)]
public record CreateUserCommand(
    string UserName,
    string Email,
    string Password,
    string? FirstName = null,
    string? LastName = null,
    string? Role = null)
    : IRequest<UserDto>;

/// <summary>
/// Handler for CreateUserCommand
/// </summary>
public class CreateUserCommandHandler(IUserService userService)
    : IRequestHandler<CreateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        return await userService.CreateUserAsync(
            request.UserName,
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName,
            request.Role,
            cancellationToken);
    }
}
