using PoopNPour.Abstractions.Identity;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.Users.Exceptions;
using PoopNPour.Domain.Common.Auth;
using PoopNPour.Domain.Common.Identity;

namespace PoopNPour.Infrastructure.Repositories.UserRepository;

/// <summary>
/// Implementation of IUserService using IIdentityService.
/// Handles mapping from ApplicationUser to UserDto.
/// </summary>
public class UserRepositoryService(IIdentityService identityService) : IUserService
{
    #region Getting

    public async Task<UserDto?> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await identityService.GetUserByIdAsync(userId, cancellationToken);
        if (user == null) return null;

        var roles = await identityService.GetUserRolesAsync(user, cancellationToken);
        return MapToDto(user, roles);
    }

    public async Task<UserDto?> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        var user = await identityService.GetUserByUserNameAsync(userName, cancellationToken);
        if (user == null) return null;

        var roles = await identityService.GetUserRolesAsync(user, cancellationToken);
        return MapToDto(user, roles);
    }

    public async Task<UserDto?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await identityService.GetUserByEmailAsync(email, cancellationToken);
        if (user == null) return null;

        var roles = await identityService.GetUserRolesAsync(user, cancellationToken);
        return MapToDto(user, roles);
    }

    public async Task<(IEnumerable<UserDto> Users, int TotalCount)> GetUsersAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var (users, totalCount) = await identityService.GetUsersAsync(
            pageNumber,
            pageSize,
            searchTerm,
            cancellationToken);

        var userDtos = new List<UserDto>();
        foreach (var user in users)
        {
            var roles = await identityService.GetUserRolesAsync(user, cancellationToken);
            userDtos.Add(MapToDto(user, roles));
        }

        return (userDtos, totalCount);
    }

    public async Task<UserDto?> ValidatePasswordAndGetUserAsync(
        string userNameOrEmail,
        string password,
        CancellationToken cancellationToken = default)
    {
        var user = await identityService.ValidatePasswordAsync(
            userNameOrEmail,
            password,
            cancellationToken);
        if (user == null) return null;

        var roles = await identityService.GetUserRolesAsync(user, cancellationToken);
        return MapToDto(user, roles);
    }

    #endregion Getting

    #region Updating

    public async Task<UserDto> CreateUserAsync(
        string userName,
        string email,
        string password,
        string? firstName = null,
        string? lastName = null,
        string? role = null,
        CancellationToken cancellationToken = default)
    {
        var user = await identityService.CreateUserAsync(
            userName,
            email,
            password,
            firstName,
            lastName,
            cancellationToken);

        await identityService.AddUserToRoleAsync(user, role ?? Roles.Web_Api, cancellationToken);

        var roles = await identityService.GetUserRolesAsync(user, cancellationToken);
        return MapToDto(user, roles);
    }

    public async Task DeleteUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await identityService.GetUserByIdAsync(userId, cancellationToken);
        if (user == null)
            throw new UserNotFoundException(userId);

        await identityService.DeleteUserAsync(userId, cancellationToken);
    }

    public async Task<UserDto> UpdateUserAsync(
        string userId,
        string? firstName = null,
        string? lastName = null,
        string? email = null,
        CancellationToken cancellationToken = default)
    {
        var user = await identityService.GetUserByIdAsync(userId, cancellationToken);
        if (user == null)
            throw new UserNotFoundException(userId);

        if (!string.IsNullOrEmpty(email) && email != user.Email)
        {
            var existingUser = await identityService.GetUserByEmailAsync(email, cancellationToken);
            if (existingUser != null && existingUser.Id != userId)
                throw new EmailAlreadyInUseException(email);
            user.Email = email;
            user.NormalizedEmail = email.ToUpperInvariant();
        }

        if (!string.IsNullOrEmpty(firstName))
            user.FirstName = firstName;

        if (!string.IsNullOrEmpty(lastName))
            user.LastName = lastName;

        user.UpdatedAt = DateTime.UtcNow;

        var updatedUser = await identityService.UpdateUserAsync(user, cancellationToken);
        var roles = await identityService.GetUserRolesAsync(updatedUser, cancellationToken);
        return MapToDto(updatedUser, roles);
    }

    #endregion Updating

    #region Claims

    public async Task<IList<ClaimDto>> GetUserClaimsAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await identityService.GetUserByIdAsync(userId, cancellationToken);
        if (user == null)
            throw new UserNotFoundException(userId);

        return await identityService.GetUserClaimsAsync(user, cancellationToken);
    }

    public async Task<IList<ClaimDto>> AddUserClaimsAsync(string userId, IEnumerable<ClaimDto> claims, CancellationToken cancellationToken = default)
    {
        var user = await identityService.GetUserByIdAsync(userId, cancellationToken);
        if (user == null)
            throw new UserNotFoundException(userId);

        await identityService.AddUserClaimsAsync(user, claims, cancellationToken);
        return await identityService.GetUserClaimsAsync(user, cancellationToken);
    }

    public async Task<IList<ClaimDto>> RemoveUserClaimsAsync(string userId, IEnumerable<ClaimDto> claims, CancellationToken cancellationToken = default)
    {
        var user = await identityService.GetUserByIdAsync(userId, cancellationToken);
        if (user == null)
            throw new UserNotFoundException(userId);

        await identityService.RemoveUserClaimsAsync(user, claims, cancellationToken);
        return await identityService.GetUserClaimsAsync(user, cancellationToken);
    }

    #endregion Claims

    #region Mapping

    private static UserDto MapToDto(ApplicationUser user, IEnumerable<string> roles) => new()
    {
        Id = user.Id ?? string.Empty,
        Email = user.Email ?? string.Empty,
        UserName = user.UserName ?? string.Empty,
        FirstName = user.FirstName,
        LastName = user.LastName,
        CreatedAt = user.CreatedAt,
        UpdatedAt = user.UpdatedAt,
        Roles = roles
    };

    #endregion Mapping
}
