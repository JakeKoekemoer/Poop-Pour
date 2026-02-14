using Microsoft.EntityFrameworkCore;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Application.FamilyUsers.Exceptions;
using PoopNPour.Application.FamilyUsers.Models;
using PoopNPour.Infrastructure.Data;

namespace PoopNPour.Infrastructure.Services.FamilyUserService;

/// <summary>
/// Service implementation for managing family users
/// </summary>
public class FamilyUserService(ApplicationDbContext context) : IFamilyUserService
{
    public async Task<FamilyUserDto> AddUserToFamilyAsync(
        Guid familyId,
        string userId,
        CancellationToken cancellationToken = default)
    {
        // Check if already exists
        var existing = await context.FamilyUsers
            .FirstOrDefaultAsync(fu => fu.FamilyId == familyId && fu.UserId == userId, cancellationToken);

        if (existing != null)
        {
            throw new UserAlreadyInFamilyException(familyId, userId);
        }

        var familyUser = new Domain.Entities.FamilyUser
        {
            FamilyId = familyId,
            UserId = userId
        };

        context.FamilyUsers.Add(familyUser);
        await context.SaveChangesAsync(cancellationToken);

        return MapToDto(familyUser);
    }

    public async Task<bool> RemoveUserFromFamilyAsync(
        Guid familyId,
        string userId,
        CancellationToken cancellationToken = default)
    {
        var familyUser = await context.FamilyUsers
            .FirstOrDefaultAsync(fu => fu.FamilyId == familyId && fu.UserId == userId, cancellationToken);

        if (familyUser == null)
        {
            return false;
        }

        context.FamilyUsers.Remove(familyUser);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<FamilyUserDto?> GetFamilyUserByIdAsync(
        Guid familyId,
        string userId,
        CancellationToken cancellationToken = default)
    {
        var familyUser = await context.FamilyUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(fu => fu.FamilyId == familyId && fu.UserId == userId, cancellationToken);

        return familyUser == null ? null : MapToDto(familyUser);
    }

    public async Task<(IEnumerable<FamilyUserDto> familyUsers, int totalCount)> GetFamilyUsersAsync(
        int pageNumber,
        int pageSize,
        Guid? familyId = null,
        string? userId = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.FamilyUsers.AsNoTracking();

        // Apply filters
        if (familyId.HasValue)
        {
            query = query.Where(fu => fu.FamilyId == familyId.Value);
        }

        if (!string.IsNullOrWhiteSpace(userId))
        {
            query = query.Where(fu => fu.UserId == userId);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var familyUsers = await query
            .OrderBy(fu => fu.FamilyId)
            .ThenBy(fu => fu.UserId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(fu => MapToDto(fu))
            .ToListAsync(cancellationToken);

        return (familyUsers, totalCount);
    }

    private static FamilyUserDto MapToDto(Domain.Entities.FamilyUser familyUser)
    {
        return new FamilyUserDto
        {
            FamilyId = familyUser.FamilyId,
            UserId = familyUser.UserId,
            CreatedOn = familyUser.CreatedOn,
            CreatedBy = familyUser.CreatedBy,
            LastModifiedOn = familyUser.LastModifiedOn,
            LastModifiedBy = familyUser.LastModifiedBy
        };
    }
}
