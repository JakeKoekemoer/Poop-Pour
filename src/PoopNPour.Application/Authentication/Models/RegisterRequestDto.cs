namespace PoopNPour.Application.Authentication.Models;

/// <summary>
/// Registration request data transfer object
/// </summary>
public class RegisterRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
