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

[AuthorizeFamilyMember(FamilyRole.Member)]
public record SufficientRoleRequest(Guid FamilyId) : IRequest<string>, IFamilyRequest;

public class FamilyAuthorizationBehaviorSufficientRoleProceeds
{
    [Fact]
    public async Task Handle_FamilyIdInJwtAndRoleSufficient_ProceedsToHandler()
    {
        var familyId = Guid.NewGuid();
        const string userId = "user-1";

        var user = CurrentUserBuilder.CreateAuthenticated(userId)
            .WithFamilyIds([familyId]);

        var familyUserDto = new FamilyUserDtoBuilder()
            .WithFamilyId(familyId)
            .WithUserId(userId)
            .WithRole(FamilyRole.Member)
            .Build();

        var familyUserService = Substitute.For<IFamilyUserService>();
        familyUserService
            .GetFamilyUserByIdAsync(familyId, userId, Arg.Any<CancellationToken>())
            .Returns(familyUserDto);

        var behavior = new FamilyAuthorizationBehavior<SufficientRoleRequest, string>(user, familyUserService);
        var request = new SufficientRoleRequest(familyId);

        var result = await behavior.Handle(request, _ => Task.FromResult("result"), CancellationToken.None);

        result.Should().Be("result");
    }
}
