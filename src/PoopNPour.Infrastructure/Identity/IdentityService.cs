using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PoopNPour.Application.Identity;
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

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

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
}
