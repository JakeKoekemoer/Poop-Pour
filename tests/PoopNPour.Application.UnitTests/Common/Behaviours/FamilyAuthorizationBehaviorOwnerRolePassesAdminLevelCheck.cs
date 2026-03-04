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

// Requires Admin minimum
[AuthorizeFamilyMember(FamilyRole.Admin)]
public record AdminLevelRequest(Guid FamilyId) : IRequest<string>, IFamilyRequest;

public class FamilyAuthorizationBehaviorOwnerRolePassesAdminLevelCheck
{
    [Fact]
    public async Task Handle_UserHasOwnerRoleAndAttributeRequiresAdmin_Proceeds()
    {
        var familyId = Guid.NewGuid();
        const string userId = "user-1";

        var user = CurrentUserBuilder.CreateAuthenticated(userId)
            .WithFamilyIds([familyId]);

        // Owner (30) >= Admin (20) → should pass
        var familyUserDto = new FamilyUserDtoBuilder()
            .WithFamilyId(familyId)
            .WithUserId(userId)
            .WithRole(FamilyRole.Owner)
            .Build();

        var familyUserService = Substitute.For<IFamilyUserService>();
        familyUserService
            .GetFamilyUserByIdAsync(familyId, userId, Arg.Any<CancellationToken>())
            .Returns(familyUserDto);

        var behavior = new FamilyAuthorizationBehavior<AdminLevelRequest, string>(user, familyUserService);
        var request = new AdminLevelRequest(familyId);

        var result = await behavior.Handle(request, _ => Task.FromResult("result"), CancellationToken.None);

        result.Should().Be("result");
    }
}
