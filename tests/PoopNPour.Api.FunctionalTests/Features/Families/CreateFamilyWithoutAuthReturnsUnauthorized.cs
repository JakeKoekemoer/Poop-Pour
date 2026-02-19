using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Families.Commands.CreateFamily;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Families;

public class CreateFamilyWithoutAuthReturnsUnauthorized : AuthenticatedTestBase
{
    public CreateFamilyWithoutAuthReturnsUnauthorized(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task CreateFamily_WithoutAuth_ReturnsUnauthorized()
    {
        var client = Factory.CreateClient();
        var command = new CreateFamilyCommand("TestFamily", "TestLastName");

        var response = await client.PostAsJsonAsync("/api/families", command);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
