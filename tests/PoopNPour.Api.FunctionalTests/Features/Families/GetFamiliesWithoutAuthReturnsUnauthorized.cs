using System.Net;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Families;

public class GetFamiliesWithoutAuthReturnsUnauthorized : AuthenticatedTestBase
{
    public GetFamiliesWithoutAuthReturnsUnauthorized(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task GetFamilies_WithoutAuth_ReturnsUnauthorized()
    {
        var client = Factory.CreateClient();

        var response = await client.GetAsync("/api/families");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
