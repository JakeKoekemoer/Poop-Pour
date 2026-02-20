using FluentValidation.TestHelper;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class RemoveUserRoleValidatorEmptyRoleFailsValidation
{
    private readonly RemoveUserRoleCommandValidator _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_EmptyRole_FailsValidation(string role)
    {
        var command = new RemoveUserRoleCommand(Guid.NewGuid().ToString(), role);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Role)
            .WithErrorMessage("Role is required.");
    }
}
