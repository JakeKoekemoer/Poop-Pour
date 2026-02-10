using PoopNPour.Abstractions.User;

namespace PoopNPour.Application.Authentication.Models;

/// <summary>
/// Registration response data transfer object
/// </summary>
public class RegisterResponseDto
{
    public UserDto User { get; set; } = null!;
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
