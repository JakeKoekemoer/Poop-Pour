using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using PoopNPour.Infrastructure.Identity;
using System.Security.Claims;
using Xunit;
using DomainClaimTypes = PoopNPour.Domain.Common.Auth.ClaimTypes;

namespace PoopNPour.Infrastructure.UnitTests.Identity;

public class CurrentUserFamilyIdsWithMultipleClaimsReturnsAllGuids
{
    [Fact]
    public void FamilyIds_MultipleFamilyMemberClaims_ReturnsAllGuids()
    {
        var familyId1 = Guid.NewGuid();
        var familyId2 = Guid.NewGuid();
        var familyId3 = Guid.NewGuid();

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(DomainClaimTypes.FamilyMember, familyId1.ToString()),
                new Claim(DomainClaimTypes.FamilyMember, familyId2.ToString()),
                new Claim(DomainClaimTypes.FamilyMember, familyId3.ToString())
            ],
            "Test"));

        var httpContext = Substitute.For<HttpContext>();
        httpContext.User.Returns(claimsPrincipal);

        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        httpContextAccessor.HttpContext.Returns(httpContext);

        var sut = new CurrentUser(httpContextAccessor);

        sut.FamilyIds.Should().HaveCount(3)
            .And.Contain(familyId1)
            .And.Contain(familyId2)
            .And.Contain(familyId3);
    }
}
