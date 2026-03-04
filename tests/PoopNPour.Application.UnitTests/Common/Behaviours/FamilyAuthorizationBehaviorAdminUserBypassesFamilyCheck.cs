using FluentAssertions;
using MediatR;
using NSubstitute;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Common.Behaviours;
using PoopNPour.Application.Common.Interfaces;
using PoopNPour.Application.UnitTests.TestHelpers;
using PoopNPour.Domain.Common.Auth;
using Xunit;

namespace PoopNPour.Application.UnitTests.Common.Behaviours;

[AuthorizeFamilyMember(FamilyRole.Owner)]
public record AdminBypassRequest(Guid FamilyId) : IRequest<string>, IFamilyRequest;

public class FamilyAuthorizationBehaviorAdminUserBypassesFamilyCheck
{
    [Fact]
    public async Task Handle_AdminUser_ProceedsWithoutJwtOrDbCheck()
    {
        var admin = CurrentUserBuilder.CreateAdmin();
        var familyUserService = Substitute.For<IFamilyUserService>();
        var behavior = new FamilyAuthorizationBehavior<AdminBypassRequest, string>(admin, familyUserService);
        var request = new AdminBypassRequest(Guid.NewGuid());

        var result = await behavior.Handle(request, _ => Task.FromResult("result"), CancellationToken.None);

        result.Should().Be("result");
        await familyUserService.DidNotReceive().GetFamilyUserByIdAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }
}
