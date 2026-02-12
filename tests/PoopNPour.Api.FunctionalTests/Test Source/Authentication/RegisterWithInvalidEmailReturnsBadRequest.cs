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
    public async Task Register_WithInvalidEmail_ReturnsBadRequest()
    {
        // Arrange - Email validation happens via FluentValidation in the pipeline
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

        // Assert - FluentValidation returns 400 Bad Request for invalid email
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
