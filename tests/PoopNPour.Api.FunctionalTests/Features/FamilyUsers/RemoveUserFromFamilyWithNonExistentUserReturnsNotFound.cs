using System.Net;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.FamilyUsers;

public class RemoveUserFromFamilyWithNonExistentUserReturnsNotFound : AuthenticatedTestBase
{
    public RemoveUserFromFamilyWithNonExistentUserReturnsNotFound(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task RemoveUserFromFamily_WithNonExistentUser_ReturnsNotFound()
    {
        using var client = await CreateAdminClientAsync();
        var nonExistentFamilyId = Guid.NewGuid();
        var nonExistentUserId = Guid.NewGuid().ToString();

        var response = await client.DeleteAsync($"/api/family-users/{nonExistentFamilyId}/users/{nonExistentUserId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
