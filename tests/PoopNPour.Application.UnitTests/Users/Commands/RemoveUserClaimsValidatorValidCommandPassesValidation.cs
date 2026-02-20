using FluentValidation.TestHelper;
using PoopNPour.Application.UnitTests.TestHelpers;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class RemoveUserClaimsValidatorValidCommandPassesValidation
{
    private readonly RemoveUserClaimsCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_PassesValidation()
    {
        var command = new RemoveUserClaimsCommand(
            "user-123",
            ClaimDtoBuilder.BuildList(("department", "engineering")));
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
