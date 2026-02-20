using FluentValidation.TestHelper;
using PoopNPour.Application.UnitTests.TestHelpers;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class AddUserClaimsValidatorValidCommandPassesValidation
{
    private readonly AddUserClaimsCommandValidator _validator = new();

    [Fact]
    public void Validate_SingleValidClaim_PassesValidation()
    {
        var command = new AddUserClaimsCommand(
            "user-123",
            ClaimDtoBuilder.BuildList(("department", "engineering")));
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_MultiplValidClaims_PassesValidation()
    {
        var command = new AddUserClaimsCommand(
            "user-123",
            ClaimDtoBuilder.BuildList(("department", "engineering"), ("level", "senior")));
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
