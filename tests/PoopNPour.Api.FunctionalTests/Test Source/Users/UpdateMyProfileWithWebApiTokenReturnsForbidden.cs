using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Users.Models;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Users;

public class UpdateMyProfileWithWebApiTokenReturnsForbidden : AuthenticatedTestBase
{
    public UpdateMyProfileWithWebApiTokenReturnsForbidden(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task UpdateMyProfile_WithWebApiToken_ReturnsForbidden()
    {
        // Arrange - Web API token doesn't have Can_ManageOwnProfile policy
        var client = CreateWebApiClient();
        var request = new UpdateProfileRequestDto
        {
            FirstName = "Test"
        };

        // Act
        var response = await client.PutAsJsonAsync("/api/users/me", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
