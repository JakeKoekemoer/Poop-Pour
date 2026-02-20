using PoopNPour.Abstractions.User;

namespace PoopNPour.Application.UnitTests.TestHelpers;

/// <summary>
/// Fluent builder for ClaimDto in tests.
/// </summary>
public class ClaimDtoBuilder
{
    private string _type = "custom_claim";
    private string _value = "claim_value";

    public ClaimDtoBuilder WithType(string type)
    {
        _type = type;
        return this;
    }

    public ClaimDtoBuilder WithValue(string value)
    {
        _value = value;
        return this;
    }

    public ClaimDto Build() => new() { Type = _type, Value = _value };

    /// <summary>
    /// Creates a list of ClaimDto instances for convenience.
    /// </summary>
    public static IList<ClaimDto> BuildList(params (string Type, string Value)[] claims) =>
        claims.Select(c => new ClaimDto { Type = c.Type, Value = c.Value }).ToList();
}
