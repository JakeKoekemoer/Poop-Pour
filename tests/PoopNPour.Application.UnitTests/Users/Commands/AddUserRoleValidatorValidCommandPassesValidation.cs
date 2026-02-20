using FluentValidation.TestHelper;
using PoopNPour.Application.Users.Commands;
using PoopNPour.Domain.Common.Auth;
using Xunit;

namespace PoopNPour.Application.UnitTests.Users.Commands;

public class AddUserRoleValidatorValidCommandPassesValidation
{
    private readonly AddUserRoleCommandValidator _validator = new();

    [Theory]
    [InlineData(Roles.Administrator)]
    [InlineData(Roles.Family_Head)]
    [InlineData(Roles.Family_Member)]
    [InlineData(Roles.Web_Api)]
    [InlineData(Roles.Mobile_Api)]
    public void Validate_KnownRole_PassesValidation(string role)
    {
        var command = new AddUserRoleCommand(Guid.NewGuid().ToString(), role);
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
