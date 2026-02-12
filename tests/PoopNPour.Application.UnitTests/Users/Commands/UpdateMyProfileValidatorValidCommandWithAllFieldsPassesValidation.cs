using FluentAssertions;
using FluentValidation.TestHelper;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class UpdateMyProfileValidatorValidCommandWithAllFieldsPassesValidation
{
    private readonly UpdateMyProfileCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommandWithAllFields_PassesValidation()
    {
        // Arrange
        var command = new UpdateMyProfileCommand(
            FirstName: "John",
            LastName: "Doe",
            Email: "john.doe@example.com");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
