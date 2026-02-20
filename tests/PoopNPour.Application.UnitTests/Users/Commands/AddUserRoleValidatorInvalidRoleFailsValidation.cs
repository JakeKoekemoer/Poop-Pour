using FluentValidation.TestHelper;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class AddUserRoleValidatorInvalidRoleFailsValidation
{
    private readonly AddUserRoleCommandValidator _validator = new();

    [Theory]
    [InlineData("HackerRole")]
    [InlineData("superadmin")]
    [InlineData("unknown_role")]
    public void Validate_UnknownRole_FailsValidation(string role)
    {
        var command = new AddUserRoleCommand(Guid.NewGuid().ToString(), role);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Role);
    }
}
