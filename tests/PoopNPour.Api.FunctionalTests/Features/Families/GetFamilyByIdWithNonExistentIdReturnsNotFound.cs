using System.Net;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Families;

public class GetFamilyByIdWithNonExistentIdReturnsNotFound : AuthenticatedTestBase
{
    public GetFamilyByIdWithNonExistentIdReturnsNotFound(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task GetFamilyById_WithNonExistentId_ReturnsNotFound()
    {
        using var client = await CreateAdminClientAsync();
        var nonExistentId = Guid.NewGuid();

        var response = await client.GetAsync($"/api/families/{nonExistentId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
