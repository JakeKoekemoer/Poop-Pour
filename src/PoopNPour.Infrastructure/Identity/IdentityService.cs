using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Domain.Common.Identity;
using PoopNPour.Infrastructure.Data;

namespace PoopNPour.Infrastructure.Identity;

/// <summary>
/// Implementation of IIdentityService using ASP.NET Core Identity
/// </summary>
public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly IAuthorizationService _authorizationService;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context,
        IAuthorizationService authorizationService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _authorizationService = authorizationService;
    }

    #region Authorization (Used by AuthorizationBehavior)

    /// <summary>
    /// Checks if a user is in a specific role (by user ID)
    /// </summary>
    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return false;

        return await _userManager.IsInRoleAsync(user, role);
    }

    /// <summary>
    /// Checks if a user satisfies a specific policy
    /// </summary>
    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return false;

        // Create a ClaimsPrincipal from the user
        var claims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        
        var identity = new System.Security.Claims.ClaimsIdentity(claims, "Identity");
        identity.AddClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id));
        
        foreach (var role in roles)
        {
            identity.AddClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, role));
        }

        var principal = new System.Security.Claims.ClaimsPrincipal(identity);
        
        var result = await _authorizationService.AuthorizeAsync(principal, policyName);
        return result.Succeeded;
    }

    /// <summary>
    /// Gets all roles for a user (by user ID)
    /// </summary>
    public async Task<IList<string>> GetUserRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Array.Empty<string>();

        return await _userManager.GetRolesAsync(user);
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

        var result = await _userManager.CreateAsync(user, password);
        
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
        return await _userManager.FindByNameAsync(userName);
    }

    public async Task<ApplicationUser?> GetUserByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<ApplicationUser?> GetUserByIdAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        return await _userManager.FindByIdAsync(userId);
    }

    public async Task<bool> UserExistsByUserNameAsync(
        string userName,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByNameAsync(userName);
        return user != null;
    }

    public async Task<bool> UserExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user != null;
    }

    public async Task AddUserToRoleAsync(
        ApplicationUser user,
        string role,
        CancellationToken cancellationToken = default)
    {
        await EnsureRoleExistsAsync(role, cancellationToken);
        
        if (!await _userManager.IsInRoleAsync(user, role))
        {
            var result = await _userManager.AddToRoleAsync(user, role);
            
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
        return await _userManager.IsInRoleAsync(user, role);
    }

    public async Task EnsureRoleExistsAsync(
        string role,
        CancellationToken cancellationToken = default)
    {
        if (!await _roleManager.RoleExistsAsync(role))
        {
            var identityRole = new IdentityRole(role);
            var result = await _roleManager.CreateAsync(identityRole);
            
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
        var user = await _userManager.FindByEmailAsync(userNameOrEmail) 
                   ?? await _userManager.FindByNameAsync(userNameOrEmail);

        if (user == null)
        {
            return null;
        }

        // Validate password
        var isValid = await _userManager.CheckPasswordAsync(user, password);
        
        return isValid ? user : null;
    }

    public async Task<(IEnumerable<ApplicationUser> Users, int TotalCount)> GetUsersAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = _userManager.Users.AsQueryable();

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
        return await _userManager.GetRolesAsync(user);
    }

    public async Task<ApplicationUser> UpdateUserAsync(
        ApplicationUser user,
        CancellationToken cancellationToken = default)
    {
        var result = await _userManager.UpdateAsync(user);
        
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
            throw new InvalidOperationException($"Failed to update user: {errors}");
        }

        return user;
    }
}
