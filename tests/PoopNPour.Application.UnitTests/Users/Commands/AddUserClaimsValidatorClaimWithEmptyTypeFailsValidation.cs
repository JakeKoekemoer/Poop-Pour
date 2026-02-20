using FluentValidation.TestHelper;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class AddUserClaimsValidatorClaimWithEmptyTypeFailsValidation
{
    private readonly AddUserClaimsCommandValidator _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_ClaimWithEmptyType_FailsValidation(string type)
    {
        var command = new AddUserClaimsCommand(
            "user-123",
            new[] { new ClaimDto { Type = type, Value = "some_value" } });
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor("Claims[0].Type")
            .WithErrorMessage("Claim type is required.");
    }
}
