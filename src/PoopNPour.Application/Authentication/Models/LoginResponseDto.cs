using PoopNPour.Abstractions.User;

namespace PoopNPour.Application.Authentication.Models;

/// <summary>
/// Login response data transfer object
/// </summary>
public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserDto User { get; set; } = null!;
}
