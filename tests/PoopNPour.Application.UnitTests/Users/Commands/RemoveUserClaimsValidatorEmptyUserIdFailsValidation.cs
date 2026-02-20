using FluentValidation.TestHelper;
using PoopNPour.Application.UnitTests.TestHelpers;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class RemoveUserClaimsValidatorEmptyUserIdFailsValidation
{
    private readonly RemoveUserClaimsCommandValidator _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_EmptyUserId_FailsValidation(string userId)
    {
        var command = new RemoveUserClaimsCommand(
            userId,
            ClaimDtoBuilder.BuildList(("department", "engineering")));
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UserId)
            .WithErrorMessage("User ID is required.");
    }
}
