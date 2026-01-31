namespace PoopNPour.Application.Users.Models;

/// <summary>
/// Update profile request data transfer object
/// </summary>
public class UpdateProfileRequestDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
}
