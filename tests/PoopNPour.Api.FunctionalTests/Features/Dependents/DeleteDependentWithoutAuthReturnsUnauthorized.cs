using System.Net;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Dependents;

public class DeleteDependentWithoutAuthReturnsUnauthorized : AuthenticatedTestBase
{
    public DeleteDependentWithoutAuthReturnsUnauthorized(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task DeleteDependent_WithoutAuth_ReturnsUnauthorized()
    {
        var client = Factory.CreateClient();
        var dependentId = Guid.NewGuid();

        var response = await client.DeleteAsync($"/api/dependents/{dependentId}");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
