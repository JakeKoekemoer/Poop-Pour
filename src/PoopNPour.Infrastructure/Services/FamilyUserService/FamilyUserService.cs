using Microsoft.EntityFrameworkCore;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Application.FamilyUsers.Exceptions;
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
        var familyExists = await context.Families.AnyAsync(f => f.FamilyId == familyId, cancellationToken);
        if (!familyExists)
        {
            throw new Application.Families.Exceptions.FamilyNotFoundException(familyId);
        }

        var existing = await context.FamilyUsers
            .FirstOrDefaultAsync(fu => fu.FamilyId == familyId && fu.UserId == userId, cancellationToken);

        if (existing != null)
        {
            throw new UserAlreadyInFamilyException(familyId, userId);
        }

        var familyUser = new Domain.Entities.FamilyUser
        {
            FamilyId = familyId,
            UserId = userId,
            Role = Domain.Common.Auth.FamilyRole.Member
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

        var entities = await query
            .OrderBy(fu => fu.FamilyId)
            .ThenBy(fu => fu.UserId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (entities.Select(MapToDto), totalCount);
    }

    public async Task<IEnumerable<FamilyUserDto>> GetUserFamilyMembershipsAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        var memberships = await context.FamilyUsers
            .AsNoTracking()
            .Where(fu => fu.UserId == userId)
            .ToListAsync(cancellationToken);

        return memberships.Select(MapToDto);
    }

    public async Task<(IEnumerable<FamilyMemberDto> members, int totalCount)> GetFamilyMembersAsync(
        Guid familyId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = context.FamilyUsers
            .AsNoTracking()
            .Include(fu => fu.User)
            .Include(fu => fu.Family)
            .Where(fu => fu.FamilyId == familyId);

        var totalCount = await query.CountAsync(cancellationToken);

        var entities = await query
            .OrderBy(fu => fu.CreatedOn)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (entities.Select(MapToMemberDto), totalCount);
    }

    public async Task<FamilyMemberDto?> GetFamilyMemberAsync(
        Guid familyId,
        string userId,
        CancellationToken cancellationToken = default)
    {
        var familyUser = await context.FamilyUsers
            .AsNoTracking()
            .Include(fu => fu.User)
            .Include(fu => fu.Family)
            .FirstOrDefaultAsync(fu => fu.FamilyId == familyId && fu.UserId == userId, cancellationToken);

        return familyUser == null ? null : MapToMemberDto(familyUser);
    }

    private static FamilyUserDto MapToDto(Domain.Entities.FamilyUser familyUser)
    {
        return new FamilyUserDto
        {
            FamilyId = familyUser.FamilyId,
            UserId = familyUser.UserId,
            Role = familyUser.Role,
            CreatedOn = familyUser.CreatedOn,
            CreatedBy = familyUser.CreatedBy,
            LastModifiedOn = familyUser.LastModifiedOn,
            LastModifiedBy = familyUser.LastModifiedBy
        };
    }

    private static FamilyMemberDto MapToMemberDto(Domain.Entities.FamilyUser fu) => new()
    {
        UserId         = fu.UserId,
        UserName       = fu.User?.UserName ?? string.Empty,
        Email          = fu.User?.Email ?? string.Empty,
        FirstName      = fu.User?.FirstName,
        LastName       = fu.User?.LastName,
        FamilyId       = fu.FamilyId,
        FamilyName     = fu.Family?.FamilyName ?? string.Empty,
        FamilyLastName = fu.Family?.FamilyLastName ?? string.Empty,
        Role           = fu.Role,
        JoinedOn       = fu.CreatedOn,
    };
}
