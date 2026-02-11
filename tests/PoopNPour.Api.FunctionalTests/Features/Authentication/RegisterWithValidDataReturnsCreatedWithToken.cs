using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Authentication.Models;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Authentication;

public class RegisterWithValidDataReturnsCreatedWithToken : AuthenticatedTestBase
{
    public RegisterWithValidDataReturnsCreatedWithToken(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Register_WithValidData_ReturnsCreatedWithToken()
    {
        // Arrange
        using var client = CreateWebApiClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/authentication/register",
            new RegisterRequestDto
            {
                Email = "newuser@test.poopnpour.co.za",
                UserName = "new_test_user",
                Password = "NewUser@Test123",
                FirstName = "New",
                LastName = "User"
            });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<RegisterResponseDto>();
        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrEmpty();
        result.User.UserName.Should().Be("new_test_user");
        result.User.Email.Should().Be("newuser@test.poopnpour.co.za");
    }
}
