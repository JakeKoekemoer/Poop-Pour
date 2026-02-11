using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Constants;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Users.Models;
using PoopNPour.Abstractions.User;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Users;

public class GetMyProfileAsAdminReturnsOwnProfile : AuthenticatedTestBase
{
    public GetMyProfileAsAdminReturnsOwnProfile(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task GetMyProfile_AsAdmin_ReturnsOwnProfile()
    {
        // Arrange - login as admin (has Administrator role -> satisfies Can_ManageOwnProfile)
        using var client = await CreateAdminClientAsync();

        // Act
        var response = await client.GetAsync("/api/users/me");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var profile = await response.Content.ReadFromJsonAsync<UserDto>();
        profile.Should().NotBeNull();
        profile!.UserName.Should().Be(TestUsers.Admin.UserName);
        profile.Email.Should().Be(TestUsers.Admin.Email);
        profile.FirstName.Should().Be(TestUsers.Admin.FirstName);
        profile.LastName.Should().Be(TestUsers.Admin.LastName);
    }
}
