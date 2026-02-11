namespace PoopNPour.Application.Users.Models;

/// <summary>
/// Generic paginated response data transfer object
/// </summary>
public class PaginatedResponseDto<T>
{
    public IEnumerable<T> Items { get; set; } = Array.Empty<T>();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
