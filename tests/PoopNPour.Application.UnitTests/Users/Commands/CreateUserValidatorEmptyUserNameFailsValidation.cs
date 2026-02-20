using FluentValidation.TestHelper;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class CreateUserValidatorEmptyUserNameFailsValidation
{
    private readonly CreateUserCommandValidator _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_EmptyUserName_FailsValidation(string userName)
    {
        var command = new CreateUserCommand(userName, "user@test.com", "Password1!");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UserName)
            .WithErrorMessage("Username is required.");
    }
}
