using FluentValidation.TestHelper;
using PoopNPour.Application.Users.Commands;
using PoopNPour.Domain.Common.Auth;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class RemoveUserRoleValidatorValidCommandPassesValidation
{
    private readonly RemoveUserRoleCommandValidator _validator = new();

    [Theory]
    [InlineData(Roles.Administrator)]
    [InlineData(Roles.Tenant)]
    [InlineData(Roles.Web_Api)]
    [InlineData(Roles.Mobile_Api)]
    public void Validate_KnownRole_PassesValidation(string role)
    {
        var command = new RemoveUserRoleCommand(Guid.NewGuid().ToString(), role);
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
