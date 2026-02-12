using FluentValidation.TestHelper;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class UpdateMyProfileValidatorEmailTooLongFailsValidation
{
    private readonly UpdateMyProfileCommandValidator _validator = new();

    [Fact]
    public void Validate_EmailTooLong_FailsValidation()
    {
        // Arrange
        var longEmail = new string('a', 90) + "@example.com"; // 102 chars total
        var command = new UpdateMyProfileCommand(
            FirstName: "John",
            LastName: "Doe",
            Email: longEmail);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email cannot exceed 100 characters.");
    }
}
