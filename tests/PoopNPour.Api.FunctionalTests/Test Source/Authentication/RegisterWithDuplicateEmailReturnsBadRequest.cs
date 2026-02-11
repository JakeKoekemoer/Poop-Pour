using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Constants;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Authentication.Models;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Authentication;

public class RegisterWithDuplicateEmailReturnsBadRequest : AuthenticatedTestBase
{
    public RegisterWithDuplicateEmailReturnsBadRequest(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Register_WithDuplicateEmail_ReturnsBadRequest()
    {
        // Arrange - admin user already exists from seeding
        using var client = CreateWebApiClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/authentication/register",
            new RegisterRequestDto
            {
                Email = TestUsers.Admin.Email,
                UserName = "another_username",
                Password = "AnotherPassword@123",
                FirstName = "Duplicate",
                LastName = "User"
            });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
            "registering with an existing email should be rejected by UserAlreadyExistsException");
    }
}
