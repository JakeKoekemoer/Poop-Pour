namespace PoopNPour.Application.Authentication.Models;

/// <summary>
/// Login request data transfer object
/// </summary>
public class LoginRequestDto
{
    public string EmailOrUserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
