using System.Net;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Dependents;

public class DeleteDependentWithNonExistentIdReturnsNotFound : AuthenticatedTestBase
{
    public DeleteDependentWithNonExistentIdReturnsNotFound(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task DeleteDependent_WithNonExistentId_ReturnsNotFound()
    {
        using var client = await CreateAdminClientAsync();
        var nonExistentId = Guid.NewGuid();

        var response = await client.DeleteAsync($"/api/dependents/{nonExistentId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
