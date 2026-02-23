using FluentValidation.TestHelper;
using PoopNPour.Application.Users.Commands;
using PoopNPour.Domain.Common.Auth;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class CreateUserValidatorValidCommandPassesValidation
{
    private readonly CreateUserCommandValidator _validator = new();

    [Fact]
    public void Validate_RequiredFieldsOnly_PassesValidation()
    {
        var command = new CreateUserCommand("newuser", "new@test.com", "Password1!");
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_AllFieldsIncludingOptionals_PassesValidation()
    {
        var command = new CreateUserCommand("newuser", "new@test.com", "Password1!", "John", "Doe", Roles.Administrator);
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
