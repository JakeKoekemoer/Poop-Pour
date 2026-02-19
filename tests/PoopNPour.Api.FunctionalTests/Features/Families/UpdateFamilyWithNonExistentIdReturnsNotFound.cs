using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Families.Commands.UpdateFamily;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Families;

public class UpdateFamilyWithNonExistentIdReturnsNotFound : AuthenticatedTestBase
{
    public UpdateFamilyWithNonExistentIdReturnsNotFound(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task UpdateFamily_WithNonExistentId_ReturnsNotFound()
    {
        using var client = await CreateAdminClientAsync();
        var nonExistentId = Guid.NewGuid();
        var command = new UpdateFamilyCommand(nonExistentId, "UpdatedName", null);

        var response = await client.PutAsJsonAsync($"/api/families/{nonExistentId}", command);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
