using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Constants;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Users.Models;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Users;

public class UpdateMyProfileWithEmailAlreadyInUseReturnsBadRequest : AuthenticatedTestBase
{
    public UpdateMyProfileWithEmailAlreadyInUseReturnsBadRequest(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task UpdateMyProfile_WithEmailAlreadyInUse_ReturnsBadRequest()
    {
        // Arrange - try to use WebApiClient's email
        using var client = await CreateAdminClientAsync();
        var request = new UpdateProfileRequestDto
        {
            Email = TestUsers.WebApiClient.Email
        };

        // Act
        var response = await client.PutAsJsonAsync("/api/users/me", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
