using FluentValidation.TestHelper;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class UpdateMyProfileValidatorEmptyStringForFirstAndLastNamePassesValidation
{
    private readonly UpdateMyProfileCommandValidator _validator = new();

    [Fact]
    public void Validate_EmptyStringForFirstAndLastName_PassesValidation()
    {
        // Arrange
        var command = new UpdateMyProfileCommand(
            FirstName: "",
            LastName: "",
            Email: "valid@email.com");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
        result.ShouldNotHaveValidationErrorFor(x => x.LastName);
    }
}
