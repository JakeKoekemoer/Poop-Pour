using FluentValidation.TestHelper;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class UpdateMyProfileValidatorEmptyStringForEmailPassesValidation
{
    private readonly UpdateMyProfileCommandValidator _validator = new();

    [Fact]
    public void Validate_EmptyStringForEmail_PassesValidation()
    {
        // Arrange
        var command = new UpdateMyProfileCommand(
            FirstName: "John",
            LastName: "Doe",
            Email: "");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }
}
