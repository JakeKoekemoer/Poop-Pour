using System.Net;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Families;

public class DeleteFamilyWithNonExistentIdReturnsNotFound : AuthenticatedTestBase
{
    public DeleteFamilyWithNonExistentIdReturnsNotFound(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task DeleteFamily_WithNonExistentId_ReturnsNotFound()
    {
        using var client = await CreateAdminClientAsync();
        var nonExistentId = Guid.NewGuid();

        var response = await client.DeleteAsync($"/api/families/{nonExistentId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
