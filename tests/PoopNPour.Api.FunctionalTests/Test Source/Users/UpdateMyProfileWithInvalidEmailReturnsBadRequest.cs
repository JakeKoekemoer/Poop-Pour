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
    public async Task UpdateMyProfile_WithInvalidEmail_ReturnsBadRequest()
    {
        // Arrange - Email validation happens via FluentValidation in the pipeline
        using var client = await CreateAdminClientAsync();
        var request = new UpdateProfileRequestDto
        {
            Email = "not-a-valid-email"
        };

        // Act
        var response = await client.PutAsJsonAsync("/api/users/me", request);

        // Assert - FluentValidation returns 400 Bad Request for invalid email
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
