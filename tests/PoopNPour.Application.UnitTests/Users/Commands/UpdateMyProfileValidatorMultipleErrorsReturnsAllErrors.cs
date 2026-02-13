using FluentValidation.TestHelper;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class UpdateMyProfileValidatorMultipleErrorsReturnsAllErrors
{
    private readonly UpdateMyProfileCommandValidator _validator = new();

    [Fact]
    public void Validate_MultipleErrors_ReturnsAllErrors()
    {
        // Arrange
        var command = new UpdateMyProfileCommand(
            FirstName: new string('a', 51),
            LastName: new string('b', 51),
            Email: "invalid-email");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
}
