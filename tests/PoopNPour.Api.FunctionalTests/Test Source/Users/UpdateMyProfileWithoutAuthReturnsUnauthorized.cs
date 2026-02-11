using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Users.Models;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Users;

public class UpdateMyProfileWithoutAuthReturnsUnauthorized : AuthenticatedTestBase
{
    public UpdateMyProfileWithoutAuthReturnsUnauthorized(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task UpdateMyProfile_WithoutAuth_ReturnsUnauthorized()
    {
        // Arrange
        var client = Factory.CreateClient();
        var request = new UpdateProfileRequestDto
        {
            FirstName = "Test"
        };

        // Act
        var response = await client.PutAsJsonAsync("/api/users/me", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
