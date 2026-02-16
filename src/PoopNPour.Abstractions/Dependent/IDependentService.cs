namespace PoopNPour.Abstractions.Dependent;

/// <summary>
/// Service interface for managing dependents
/// </summary>
public interface IDependentService
{
    /// <summary>
    /// Creates a new dependent
    /// </summary>
    Task<DependentDto> CreateDependentAsync(
        Guid familyId,
        string dependentName,
        string dependentSurname,
        DateTimeOffset dateOfBirth,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing dependent
    /// </summary>
    Task<DependentDto?> UpdateDependentAsync(
        Guid dependentId,
        string? dependentName = null,
        string? dependentSurname = null,
        DateTimeOffset? dateOfBirth = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a dependent by ID
    /// </summary>
    Task<DependentDto?> GetDependentByIdAsync(
        Guid dependentId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets paginated list of dependents
    /// </summary>
    Task<(IEnumerable<DependentDto> dependents, int totalCount)> GetDependentsAsync(
        int pageNumber,
        int pageSize,
        Guid? familyId = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default);
}
