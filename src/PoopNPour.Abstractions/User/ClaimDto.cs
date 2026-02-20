namespace PoopNPour.Abstractions.User;

/// <summary>
/// Represents an identity claim with a type and value
/// </summary>
public class ClaimDto
{
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
