using FluentAssertions;
using MediatR;
using NSubstitute;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Common.Behaviours;
using PoopNPour.Application.Common.Exceptions;
using PoopNPour.Application.Common.Interfaces;
using PoopNPour.Application.UnitTests.TestHelpers;
using PoopNPour.Domain.Common.Auth;
using Xunit;

namespace PoopNPour.Application.UnitTests.Common.Behaviours;

[AuthorizeFamilyMember(FamilyRole.Member)]
public record FamilyRemovedFromDbRequest(Guid FamilyId) : IRequest<string>, IFamilyRequest;

public class FamilyAuthorizationBehaviorFamilyRemovedFromDbThrowsForbiddenAccessException
{
    [Fact]
    public async Task Handle_FamilyIdInJwtButDbRecordNull_ThrowsForbiddenAccessException()
    {
        var familyId = Guid.NewGuid();
        const string userId = "user-1";

        // Family is in JWT claims (passes stage 1)
        var user = CurrentUserBuilder.CreateAuthenticated(userId)
            .WithFamilyIds([familyId]);

        var familyUserService = Substitute.For<IFamilyUserService>();
        // DB returns null — user has been removed from the family
        familyUserService
            .GetFamilyUserByIdAsync(familyId, userId, Arg.Any<CancellationToken>())
            .Returns((FamilyUserDto?)null);

        var behavior = new FamilyAuthorizationBehavior<FamilyRemovedFromDbRequest, string>(user, familyUserService);
        var request = new FamilyRemovedFromDbRequest(familyId);

        var act = () => behavior.Handle(request, _ => Task.FromResult("result"), CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }
}
