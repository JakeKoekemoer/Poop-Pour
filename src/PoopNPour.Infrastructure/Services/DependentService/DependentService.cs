using Microsoft.EntityFrameworkCore;
using PoopNPour.Abstractions.Dependent;
using PoopNPour.Infrastructure.Data;

namespace PoopNPour.Infrastructure.Services.DependentService;

/// <summary>
/// Service implementation for managing dependents
/// </summary>
public class DependentService(ApplicationDbContext context) : IDependentService
{

    public async Task<DependentDto> CreateDependentAsync(
        Guid familyId,
        string dependentName,
        string dependentSurname,
        DateTimeOffset dateOfBirth,
        CancellationToken cancellationToken = default)
    {
        var dependent = new Domain.Entities.Dependent
        {
            FamilyId = familyId,
            DependentName = dependentName,
            DependentSurname = dependentSurname,
            DateOfBirth = dateOfBirth
        };

        context.Dependents.Add(dependent);
        await context.SaveChangesAsync(cancellationToken);

        return MapToDto(dependent);
    }

    public async Task<DependentDto?> UpdateDependentAsync(
        Guid dependentId,
        string? dependentName = null,
        string? dependentSurname = null,
        DateTimeOffset? dateOfBirth = null,
        CancellationToken cancellationToken = default)
    {
        var dependent = await context.Dependents
            .FirstOrDefaultAsync(d => d.DependentId == dependentId, cancellationToken);

        if (dependent == null)
        {
            return null;
        }

        if (dependentName != null)
            dependent.DependentName = dependentName;

        if (dependentSurname != null)
            dependent.DependentSurname = dependentSurname;

        if (dateOfBirth.HasValue)
            dependent.DateOfBirth = dateOfBirth.Value;

        await context.SaveChangesAsync(cancellationToken);

        return MapToDto(dependent);
    }

    public async Task<DependentDto?> GetDependentByIdAsync(
        Guid dependentId,
        CancellationToken cancellationToken = default)
    {
        var dependent = await context.Dependents
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.DependentId == dependentId, cancellationToken);

        return dependent == null ? null : MapToDto(dependent);
    }

    public async Task<(IEnumerable<DependentDto> dependents, int totalCount)> GetDependentsAsync(
        int pageNumber,
        int pageSize,
        Guid? familyId = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.Dependents.AsNoTracking();

        // Apply filters
        if (familyId.HasValue)
        {
            query = query.Where(d => d.FamilyId == familyId.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(d => 
                d.DependentName.Contains(searchTerm) || 
                d.DependentSurname.Contains(searchTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var entities = await query
            .OrderBy(d => d.DependentSurname)
            .ThenBy(d => d.DependentName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (entities.Select(MapToDto), totalCount);
    }

    public async Task<bool> IsDependentNameDuplicateAsync(
        Guid familyId,
        string dependentName,
        string dependentSurname,
        Guid? excludeDependentId = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.Dependents
            .Where(d => d.FamilyId == familyId &&
                        EF.Functions.Like(d.DependentName, dependentName) &&
                        EF.Functions.Like(d.DependentSurname, dependentSurname));

        if (excludeDependentId.HasValue)
        {
            query = query.Where(d => d.DependentId != excludeDependentId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    private static DependentDto MapToDto(Domain.Entities.Dependent dependent)
    {
        return new DependentDto
        {
            DependentId = dependent.DependentId,
            FamilyId = dependent.FamilyId,
            DependentName = dependent.DependentName,
            DependentSurname = dependent.DependentSurname,
            DateOfBirth = dependent.DateOfBirth,
            Age = dependent.Age,
            CreatedOn = dependent.CreatedOn,
            CreatedBy = dependent.CreatedBy,
            LastModifiedOn = dependent.LastModifiedOn,
            LastModifiedBy = dependent.LastModifiedBy
        };
    }
}
