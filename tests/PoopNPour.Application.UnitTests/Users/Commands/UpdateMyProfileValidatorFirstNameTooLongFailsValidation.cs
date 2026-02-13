using FluentValidation.TestHelper;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class UpdateMyProfileValidatorFirstNameTooLongFailsValidation
{
    private readonly UpdateMyProfileCommandValidator _validator = new();

    [Fact]
    public void Validate_FirstNameTooLong_FailsValidation()
    {
        // Arrange
        var longFirstName = new string('a', 51);
        var command = new UpdateMyProfileCommand(
            FirstName: longFirstName,
            LastName: "Doe",
            Email: "john.doe@example.com");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName)
            .WithErrorMessage("First name cannot exceed 50 characters.");
    }
}
