using FluentAssertions;
using FluentValidation.TestHelper;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class UpdateMyProfileValidatorAllNullFieldsPassesValidation
{
    private readonly UpdateMyProfileCommandValidator _validator = new();

    [Fact]
    public void Validate_AllNullFields_PassesValidation()
    {
        // Arrange
        var command = new UpdateMyProfileCommand(
            FirstName: null,
            LastName: null,
            Email: null);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
