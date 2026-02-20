using FluentValidation.TestHelper;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class UpdateUserValidatorValidCommandPassesValidation
{
    private readonly UpdateUserCommandValidator _validator = new();

    [Fact]
    public void Validate_UserIdOnly_PassesValidation()
    {
        var command = new UpdateUserCommand("user-123");
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_AllFieldsProvided_PassesValidation()
    {
        var command = new UpdateUserCommand("user-123", "John", "Doe", "john@test.com");
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
