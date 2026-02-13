using FluentValidation.TestHelper;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class UpdateMyProfileValidatorLastNameTooLongFailsValidation
{
    private readonly UpdateMyProfileCommandValidator _validator = new();

    [Fact]
    public void Validate_LastNameTooLong_FailsValidation()
    {
        // Arrange
        var longLastName = new string('a', 51);
        var command = new UpdateMyProfileCommand(
            FirstName: "John",
            LastName: longLastName,
            Email: "john.doe@example.com");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LastName)
            .WithErrorMessage("Last name cannot exceed 50 characters.");
    }
}
