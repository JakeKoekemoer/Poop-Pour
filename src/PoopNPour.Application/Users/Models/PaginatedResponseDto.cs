// This file has been moved to PoopNPour.Application.Common.Models.PaginatedResponseDto
// Keep this for backward compatibility
namespace PoopNPour.Application.Users.Models;

/// <summary>
/// Generic paginated response data transfer object
/// </summary>
[Obsolete("Use PoopNPour.Application.Common.Models.PaginatedResponseDto instead")]
public class PaginatedResponseDto<T> : Common.Models.PaginatedResponseDto<T>
{
}
