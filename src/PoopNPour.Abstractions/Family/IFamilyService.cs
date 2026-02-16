namespace PoopNPour.Abstractions.Family;

/// <summary>
/// Service interface for managing families
/// </summary>
public interface IFamilyService
{
    /// <summary>
    /// Creates a new family
    /// </summary>
    Task<FamilyDto> CreateFamilyAsync(
        string familyName,
        string familyLastName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing family
    /// </summary>
    Task<FamilyDto?> UpdateFamilyAsync(
        Guid familyId,
        string? familyName = null,
        string? familyLastName = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a family by ID
    /// </summary>
    Task<FamilyDto?> GetFamilyByIdAsync(
        Guid familyId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets paginated list of families
    /// </summary>
    Task<(IEnumerable<FamilyDto> families, int totalCount)> GetFamiliesAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        CancellationToken cancellationToken = default);
}
