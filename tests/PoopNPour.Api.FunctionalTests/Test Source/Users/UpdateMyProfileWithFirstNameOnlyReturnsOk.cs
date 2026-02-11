using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.Users.Models;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Users;

public class UpdateMyProfileWithFirstNameOnlyReturnsOk : AuthenticatedTestBase
{
    public UpdateMyProfileWithFirstNameOnlyReturnsOk(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task UpdateMyProfile_WithFirstNameOnly_ReturnsOk()
    {
        // Arrange
        using var client = await CreateAdminClientAsync();
        var request = new UpdateProfileRequestDto
        {
            FirstName = "PartialUpdate"
        };

        // Act
        var response = await client.PutAsJsonAsync("/api/users/me", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<UserDto>();
        result.Should().NotBeNull();
        result!.FirstName.Should().Be("PartialUpdate");
    }
}
