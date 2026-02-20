using FluentValidation.TestHelper;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.Users.Commands;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class RemoveUserClaimsValidatorEmptyClaimsListFailsValidation
{
    private readonly RemoveUserClaimsCommandValidator _validator = new();

    [Fact]
    public void Validate_EmptyClaimsList_FailsValidation()
    {
        var command = new RemoveUserClaimsCommand("user-123", Array.Empty<ClaimDto>());
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Claims)
            .WithErrorMessage("At least one claim must be provided.");
    }
}
