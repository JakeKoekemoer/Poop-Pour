using FluentAssertions;
using MediatR;
using NSubstitute;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Application.Common.Behaviours;
using PoopNPour.Application.UnitTests.TestHelpers;
using Xunit;

namespace PoopNPour.Application.UnitTests.Common.Behaviours;

public record FamilyRequestWithoutAttribute(Guid FamilyId) : IRequest<string>;

public class FamilyAuthorizationBehaviorRequestWithoutAttributeProceeds
{
    [Fact]
    public async Task Handle_RequestWithoutAuthorizeFamilyMemberAttribute_ProceedsWithoutChecks()
    {
        var user = CurrentUserBuilder.CreateAuthenticated("user-1");
        var familyUserService = Substitute.For<IFamilyUserService>();
        var behavior = new FamilyAuthorizationBehavior<FamilyRequestWithoutAttribute, string>(user, familyUserService);
        var request = new FamilyRequestWithoutAttribute(Guid.NewGuid());

        var result = await behavior.Handle(request, _ => Task.FromResult("result"), CancellationToken.None);

        result.Should().Be("result");
        await familyUserService.DidNotReceive().GetFamilyUserByIdAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }
}
