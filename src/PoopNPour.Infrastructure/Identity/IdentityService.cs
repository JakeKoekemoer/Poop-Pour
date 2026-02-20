using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Abstractions.User;
using PoopNPour.Domain.Common.Identity;
using PoopNPour.Infrastructure.Data;
using System.Security.Claims;

namespace PoopNPour.Infrastructure.Identity;

/// <summary>
/// Implementation of IIdentityService using ASP.NET Core Identity
/// </summary>
public class IdentityService(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    ApplicationDbContext context,
    IAuthorizationService authorizationService) : IIdentityService
{
    #region Authorization (Used by AuthorizationBehavior)

    /// <summary>
    /// Checks if a user is in a specific role (by user ID)
    /// </summary>
    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            return false;

        return await userManager.IsInRoleAsync(user, role);
    }

    /// <summary>
    /// Checks if a user satisfies a specific policy
    /// </summary>
    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            return false;

        var claims = await userManager.GetClaimsAsync(user);
        var roles = await userManager.GetRolesAsync(user);
        
        var identity = new System.Security.Claims.ClaimsIdentity(claims, "Identity");
        identity.AddClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id));
        
        foreach (var role in roles)
        {
            identity.AddClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, role));
        }

        var principal = new System.Security.Claims.ClaimsPrincipal(identity);
        
        var result = await authorizationService.AuthorizeAsync(principal, policyName);
        return result.Succeeded;
    }

    /// <summary>
    /// Gets all roles for a user (by user ID)
    /// </summary>
    public async Task<IList<string>> GetUserRolesAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            return Array.Empty<string>();

        return await userManager.GetRolesAsync(user);
    }

    #endregion

    public async Task<ApplicationUser> CreateUserAsync(
        string userName,
        string email,
        string password,
        string? firstName = null,
        string? lastName = null,
        CancellationToken cancellationToken = default)
    {
        var user = new ApplicationUser
        {
            UserName = userName,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            CreatedAt = DateTime.UtcNow,
            EmailConfirmed = true // Auto-confirm for seeded users
        };

        var result = await userManager.CreateAsync(user, password);
        
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
            throw new InvalidOperationException($"Failed to create user: {errors}");
        }

        return user;
    }

    public async Task<ApplicationUser?> GetUserByUserNameAsync(
        string userName,
        CancellationToken cancellationToken = default)
    {
        return await userManager.FindByNameAsync(userName);
    }

    public async Task<ApplicationUser?> GetUserByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return await userManager.FindByEmailAsync(email);
    }

    public async Task<ApplicationUser?> GetUserByIdAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        return await userManager.FindByIdAsync(userId);
    }

    public async Task<bool> UserExistsByUserNameAsync(
        string userName,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByNameAsync(userName);
        return user != null;
    }

    public async Task<bool> UserExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        return user != null;
    }

    public async Task AddUserToRoleAsync(
        ApplicationUser user,
        string role,
        CancellationToken cancellationToken = default)
    {
        await EnsureRoleExistsAsync(role, cancellationToken);
        
        if (!await userManager.IsInRoleAsync(user, role))
        {
            var result = await userManager.AddToRoleAsync(user, role);
            
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
                throw new InvalidOperationException($"Failed to add user to role: {errors}");
            }
        }
    }

    public async Task<bool> IsUserInRoleAsync(
        ApplicationUser user,
        string role,
        CancellationToken cancellationToken = default)
    {
        return await userManager.IsInRoleAsync(user, role);
    }

    public async Task EnsureRoleExistsAsync(
        string role,
        CancellationToken cancellationToken = default)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            var identityRole = new IdentityRole(role);
            var result = await roleManager.CreateAsync(identityRole);
            
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
                throw new InvalidOperationException($"Failed to create role: {errors}");
            }
        }
    }

    public async Task<ApplicationUser?> ValidatePasswordAsync(
        string userNameOrEmail,
        string password,
        CancellationToken cancellationToken = default)
    {
        // Try to find user by email first, then by username
        var user = await userManager.FindByEmailAsync(userNameOrEmail) 
                   ?? await userManager.FindByNameAsync(userNameOrEmail);

        if (user == null)
        {
            return null;
        }

        var isValid = await userManager.CheckPasswordAsync(user, password);
        
        return isValid ? user : null;
    }

    public async Task<(IEnumerable<ApplicationUser> Users, int TotalCount)> GetUsersAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = userManager.Users.AsQueryable();

        // Apply search filter if provided
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(u => 
                (u.Email != null && u.Email.ToLower().Contains(searchTerm)) ||
                (u.UserName != null && u.UserName.ToLower().Contains(searchTerm)) ||
                (u.FirstName != null && u.FirstName.ToLower().Contains(searchTerm)) ||
                (u.LastName != null && u.LastName.ToLower().Contains(searchTerm)));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var users = await query
            .OrderBy(u => u.UserName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (users, totalCount);
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(
        ApplicationUser user,
        CancellationToken cancellationToken = default)
    {
        return await userManager.GetRolesAsync(user);
    }

    public async Task<ApplicationUser> UpdateUserAsync(
        ApplicationUser user,
        CancellationToken cancellationToken = default)
    {
        var result = await userManager.UpdateAsync(user);
        
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
            throw new InvalidOperationException($"Failed to update user: {errors}");
        }

        return user;
    }

    public async Task DeleteUserAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId)
            ?? throw new InvalidOperationException($"User with ID '{userId}' was not found.");

        var result = await userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
            throw new InvalidOperationException($"Failed to delete user: {errors}");
        }
    }

    #region Claims Management

    public async Task<IList<ClaimDto>> GetUserClaimsAsync(
        ApplicationUser user,
        CancellationToken cancellationToken = default)
    {
        var claims = await userManager.GetClaimsAsync(user);
        return claims.Select(c => new ClaimDto { Type = c.Type, Value = c.Value }).ToList();
    }

    public async Task AddUserClaimsAsync(
        ApplicationUser user,
        IEnumerable<ClaimDto> claims,
        CancellationToken cancellationToken = default)
    {
        var identityClaims = claims.Select(c => new Claim(c.Type, c.Value)).ToList();
        var result = await userManager.AddClaimsAsync(user, identityClaims);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
            throw new InvalidOperationException($"Failed to add claims: {errors}");
        }
    }

    public async Task RemoveUserClaimsAsync(
        ApplicationUser user,
        IEnumerable<ClaimDto> claims,
        CancellationToken cancellationToken = default)
    {
        var identityClaims = claims.Select(c => new Claim(c.Type, c.Value)).ToList();
        var result = await userManager.RemoveClaimsAsync(user, identityClaims);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
            throw new InvalidOperationException($"Failed to remove claims: {errors}");
        }
    }

    #endregion

    public async Task SetAuthenticationTokenAsync(
        ApplicationUser user,
        string loginProvider,
        string tokenName,
        string tokenValue,
        CancellationToken cancellationToken = default)
    {
        var result = await userManager.SetAuthenticationTokenAsync(user, loginProvider, tokenName, tokenValue);
        
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
            throw new InvalidOperationException($"Failed to set authentication token: {errors}");
        }
    }

    public async Task<string?> GetAuthenticationTokenAsync(
        ApplicationUser user,
        string loginProvider,
        string tokenName,
        CancellationToken cancellationToken = default)
    {
        return await userManager.GetAuthenticationTokenAsync(user, loginProvider, tokenName);
    }
}
