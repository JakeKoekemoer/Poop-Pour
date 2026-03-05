using System.Net;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Families;

public class DeleteFamilyWithoutAuthReturnsUnauthorized : AuthenticatedTestBase
{
    public DeleteFamilyWithoutAuthReturnsUnauthorized(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task DeleteFamily_WithoutAuth_ReturnsUnauthorized()
    {
        var client = Factory.CreateClient();
        var familyId = Guid.NewGuid();

        var response = await client.DeleteAsync($"/api/families/{familyId}");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
