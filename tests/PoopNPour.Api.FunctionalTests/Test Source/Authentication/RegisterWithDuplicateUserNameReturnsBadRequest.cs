using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Constants;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Authentication.Models;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Authentication;

public class RegisterWithDuplicateUserNameReturnsBadRequest : AuthenticatedTestBase
{
    public RegisterWithDuplicateUserNameReturnsBadRequest(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Register_WithDuplicateUserName_ReturnsBadRequest()
    {
        // Arrange - admin user already exists from seeding
        using var client = CreateWebApiClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/authentication/register",
            new RegisterRequestDto
            {
                Email = "unique@test.poopnpour.co.za",
                UserName = TestUsers.Admin.UserName,
                Password = "UniquePassword@123",
                FirstName = "Duplicate",
                LastName = "UserName"
            });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
            "registering with an existing username should be rejected by UserAlreadyExistsException");
    }
}
