using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using PoopNPour.Infrastructure.Identity;
using System.Security.Claims;
using Xunit;
using DomainClaimTypes = PoopNPour.Domain.Common.Auth.ClaimTypes;

namespace PoopNPour.Infrastructure.UnitTests.Identity;

public class CurrentUserFamilyIdsWithInvalidGuidClaimSkipsInvalidEntry
{
    [Fact]
    public void FamilyIds_OneValidAndOneMalformedGuidClaim_ReturnsOnlyValidGuid()
    {
        var validFamilyId = Guid.NewGuid();

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(DomainClaimTypes.FamilyMember, validFamilyId.ToString()),
                new Claim(DomainClaimTypes.FamilyMember, "not-a-valid-guid")
            ],
            "Test"));

        var httpContext = Substitute.For<HttpContext>();
        httpContext.User.Returns(claimsPrincipal);

        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        httpContextAccessor.HttpContext.Returns(httpContext);

        var sut = new CurrentUser(httpContextAccessor);

        sut.FamilyIds.Should().ContainSingle()
            .Which.Should().Be(validFamilyId);
    }
}
