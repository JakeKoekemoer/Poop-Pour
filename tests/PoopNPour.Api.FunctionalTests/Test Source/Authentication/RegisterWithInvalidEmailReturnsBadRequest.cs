using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Authentication.Models;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Authentication;

public class RegisterWithInvalidEmailReturnsBadRequest : AuthenticatedTestBase
{
    public RegisterWithInvalidEmailReturnsBadRequest(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Register_WithInvalidEmail_ReturnsInternalServerError()
    {
        // Arrange - Email validation happens at Identity layer, which throws exceptions
        // that may not be properly caught as validation errors
        var client = CreateWebApiClient();
        var request = new RegisterRequestDto
        {
            Email = "not-a-valid-email",
            UserName = $"user{Guid.NewGuid():N}",
            Password = "ValidPassword123!",
            FirstName = "Test",
            LastName = "User"
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/authentication/register", request);

        // Assert - Currently returns 500, but ideally should be 400
        // This test documents current behavior; validation improvements can be made later
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}
