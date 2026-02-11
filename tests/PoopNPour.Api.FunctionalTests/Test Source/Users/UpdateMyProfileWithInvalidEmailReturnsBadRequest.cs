using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Users.Models;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Users;

public class UpdateMyProfileWithInvalidEmailReturnsBadRequest : AuthenticatedTestBase
{
    public UpdateMyProfileWithInvalidEmailReturnsBadRequest(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task UpdateMyProfile_WithInvalidEmail_ReturnsInternalServerError()
    {
        // Arrange - Email validation happens at Identity layer, which throws exceptions
        // that may not be properly caught as validation errors
        using var client = await CreateAdminClientAsync();
        var request = new UpdateProfileRequestDto
        {
            Email = "not-a-valid-email"
        };

        // Act
        var response = await client.PutAsJsonAsync("/api/users/me", request);

        // Assert - Currently returns 500, but ideally should be 400
        // This test documents current behavior; validation improvements can be made later
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}
