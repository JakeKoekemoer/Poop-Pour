using FluentValidation.TestHelper;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class AddUserRoleValidatorEmptyUserIdFailsValidation
{
    private readonly AddUserRoleCommandValidator _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_EmptyUserId_FailsValidation(string userId)
    {
        var command = new AddUserRoleCommand(userId, "Family_Head");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UserId)
            .WithErrorMessage("User ID is required.");
    }
}
