using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using PoopNPour.Infrastructure.Identity;
using System.Security.Claims;
using Xunit;

namespace PoopNPour.Infrastructure.UnitTests.Identity;

public class CurrentUserFamilyIdsWithNoClaimsReturnsEmptyCollection
{
    [Fact]
    public void FamilyIds_NoFamilyMemberClaims_ReturnsEmptyCollection()
    {
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim(ClaimTypes.NameIdentifier, "user-1")],
            "Test"));

        var httpContext = Substitute.For<HttpContext>();
        httpContext.User.Returns(claimsPrincipal);

        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        httpContextAccessor.HttpContext.Returns(httpContext);

        var sut = new CurrentUser(httpContextAccessor);

        sut.FamilyIds.Should().BeEmpty();
    }
}
