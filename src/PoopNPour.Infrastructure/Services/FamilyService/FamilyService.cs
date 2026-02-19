using Microsoft.EntityFrameworkCore;
using PoopNPour.Abstractions.Family;
using PoopNPour.Infrastructure.Data;

namespace PoopNPour.Infrastructure.Services.FamilyService;

/// <summary>
/// Service implementation for managing families
/// </summary>
public class FamilyService(ApplicationDbContext context) : IFamilyService
{
    public async Task<FamilyDto> CreateFamilyAsync(
        string familyName,
        string familyLastName,
        string? creatorUserId = null,
        CancellationToken cancellationToken = default)
    {
        var family = new Domain.Entities.Family
        {
            FamilyName = familyName,
            FamilyLastName = familyLastName
        };

        context.Families.Add(family);
        await context.SaveChangesAsync(cancellationToken);

        if (!string.IsNullOrEmpty(creatorUserId))
        {
            var familyUser = new Domain.Entities.FamilyUser
            {
                FamilyId = family.FamilyId,
                UserId = creatorUserId
            };

            context.FamilyUsers.Add(familyUser);
            await context.SaveChangesAsync(cancellationToken);
        }

        return MapToDto(family);
    }

    public async Task<FamilyDto?> UpdateFamilyAsync(
        Guid familyId,
        string? familyName = null,
        string? familyLastName = null,
        CancellationToken cancellationToken = default)
    {
        var family = await context.Families
            .FirstOrDefaultAsync(f => f.FamilyId == familyId, cancellationToken);

        if (family == null)
        {
            return null;
        }

        if (familyName != null)
            family.FamilyName = familyName;

        if (familyLastName != null)
            family.FamilyLastName = familyLastName;

        await context.SaveChangesAsync(cancellationToken);

        return MapToDto(family);
    }

    public async Task<FamilyDto?> GetFamilyByIdAsync(
        Guid familyId,
        CancellationToken cancellationToken = default)
    {
        var family = await context.Families
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.FamilyId == familyId, cancellationToken);

        return family == null ? null : MapToDto(family);
    }

    public async Task<(IEnumerable<FamilyDto> families, int totalCount)> GetFamiliesAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.Families.AsNoTracking();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(f => 
                f.FamilyName.Contains(searchTerm) || 
                f.FamilyLastName.Contains(searchTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var entities = await query
            .OrderBy(f => f.FamilyLastName)
            .ThenBy(f => f.FamilyName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (entities.Select(MapToDto), totalCount);
    }

    public async Task<bool> IsFamilyNameDuplicateAsync(
        string familyName,
        string userId,
        Guid? excludeFamilyId = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.FamilyUsers
            .Include(fu => fu.Family)
            .Where(fu => fu.UserId == userId && 
                         fu.Family!.FamilyName == familyName);

        if (excludeFamilyId.HasValue)
        {
            query = query.Where(fu => fu.FamilyId != excludeFamilyId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    private static FamilyDto MapToDto(Domain.Entities.Family family)
    {
        return new FamilyDto
        {
            FamilyId = family.FamilyId,
            FamilyName = family.FamilyName,
            FamilyLastName = family.FamilyLastName,
            CreatedOn = family.CreatedOn,
            CreatedBy = family.CreatedBy,
            LastModifiedOn = family.LastModifiedOn,
            LastModifiedBy = family.LastModifiedBy
        };
    }
}
