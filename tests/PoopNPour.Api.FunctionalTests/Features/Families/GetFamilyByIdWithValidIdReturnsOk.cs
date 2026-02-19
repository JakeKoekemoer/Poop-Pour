using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Abstractions.Family;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Families.Commands.CreateFamily;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Families;

public class GetFamilyByIdWithValidIdReturnsOk : AuthenticatedTestBase
{
    public GetFamilyByIdWithValidIdReturnsOk(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task GetFamilyById_WithValidId_ReturnsOk()
    {
        using var client = await CreateAdminClientAsync();
        var createCommand = new CreateFamilyCommand($"GetById-{Guid.NewGuid():N}", "LastName");
        var createResponse = await client.PostAsJsonAsync("/api/families", createCommand);
        var created = await createResponse.Content.ReadFromJsonAsync<FamilyDto>();

        var response = await client.GetAsync($"/api/families/{created!.FamilyId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<FamilyDto>();
        result!.FamilyId.Should().Be(created.FamilyId);
    }
}
