using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.FamilyUsers.Commands.AddUserToFamily;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.FamilyUsers;

public class AddUserToFamilyWithNonExistentFamilyReturnsNotFound : AuthenticatedTestBase
{
    public AddUserToFamilyWithNonExistentFamilyReturnsNotFound(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task AddUserToFamily_WithNonExistentFamily_ReturnsNotFound()
    {
        using var client = await CreateAdminClientAsync();
        var nonExistentFamilyId = Guid.NewGuid();
        var command = new AddUserToFamilyCommand(nonExistentFamilyId, "nonexistent@example.com");

        var response = await client.PostAsJsonAsync("/api/family-users", command);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
