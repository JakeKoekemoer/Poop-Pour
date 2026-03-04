using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using PoopNPour.Infrastructure.Identity;
using Xunit;

namespace PoopNPour.Infrastructure.UnitTests.Identity;

public class CurrentUserFamilyIdsWithNoHttpContextReturnsEmptyCollection
{
    [Fact]
    public void FamilyIds_NullHttpContext_ReturnsEmptyCollectionWithoutException()
    {
        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        httpContextAccessor.HttpContext.Returns((HttpContext?)null);

        var sut = new CurrentUser(httpContextAccessor);

        var act = () => sut.FamilyIds;

        act.Should().NotThrow();
        sut.FamilyIds.Should().BeEmpty();
    }
}
