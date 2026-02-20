using FluentValidation.TestHelper;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class AddUserClaimsValidatorClaimWithEmptyValueFailsValidation
{
    private readonly AddUserClaimsCommandValidator _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_ClaimWithEmptyValue_FailsValidation(string value)
    {
        var command = new AddUserClaimsCommand(
            "user-123",
            new[] { new ClaimDto { Type = "department", Value = value } });
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor("Claims[0].Value")
            .WithErrorMessage("Claim value is required.");
    }
}
