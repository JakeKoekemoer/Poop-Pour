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
public record FamilyIdNotInJwtRequest(Guid FamilyId) : IRequest<string>, IFamilyRequest;

public class FamilyAuthorizationBehaviorFamilyIdNotInJwtClaimsThrowsForbiddenAccessException
{
    [Fact]
    public async Task Handle_FamilyIdNotInJwtClaims_ThrowsForbiddenAccessExceptionWithoutDbCall()
    {
        var requestedFamilyId = Guid.NewGuid();
        var differentFamilyId = Guid.NewGuid();

        // User's JWT contains a different family ID
        var user = CurrentUserBuilder.CreateAuthenticated("user-1")
            .WithFamilyIds([differentFamilyId]);

        var familyUserService = Substitute.For<IFamilyUserService>();
        var behavior = new FamilyAuthorizationBehavior<FamilyIdNotInJwtRequest, string>(user, familyUserService);
        var request = new FamilyIdNotInJwtRequest(requestedFamilyId);

        var act = () => behavior.Handle(request, _ => Task.FromResult("result"), CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenAccessException>();
        await familyUserService.DidNotReceive().GetFamilyUserByIdAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }
}
