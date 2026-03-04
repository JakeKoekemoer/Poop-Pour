using FluentAssertions;
using MediatR;
using NSubstitute;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Common.Behaviours;
using PoopNPour.Application.UnitTests.TestHelpers;
using PoopNPour.Domain.Common.Auth;
using Xunit;

namespace PoopNPour.Application.UnitTests.Common.Behaviours;

// Has the attribute but does NOT implement IFamilyRequest
[AuthorizeFamilyMember(FamilyRole.Member)]
public record AttributeWithoutIFamilyRequest : IRequest<string>;

public class FamilyAuthorizationBehaviorRequestNotIFamilyRequestThrowsInvalidOperationException
{
    [Fact]
    public async Task Handle_RequestWithAttributeButNotIFamilyRequest_ThrowsInvalidOperationException()
    {
        var user = CurrentUserBuilder.CreateAuthenticated("user-1");
        var familyUserService = Substitute.For<IFamilyUserService>();
        var behavior = new FamilyAuthorizationBehavior<AttributeWithoutIFamilyRequest, string>(user, familyUserService);
        var request = new AttributeWithoutIFamilyRequest();

        var act = () => behavior.Handle(request, _ => Task.FromResult("result"), CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*IFamilyRequest*");
    }
}
