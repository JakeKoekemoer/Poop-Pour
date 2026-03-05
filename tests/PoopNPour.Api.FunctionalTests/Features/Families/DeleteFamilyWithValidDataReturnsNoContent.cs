using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Abstractions.Family;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Families.Commands.CreateFamily;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Families;

public class DeleteFamilyWithValidDataReturnsNoContent : AuthenticatedTestBase
{
    public DeleteFamilyWithValidDataReturnsNoContent(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task DeleteFamily_WithValidData_ReturnsNoContent()
    {
        using var client = await CreateAdminClientAsync();

        var familyCommand = new CreateFamilyCommand($"TestFamily-{Guid.NewGuid():N}", "LastName");
        var familyResponse = await client.PostAsJsonAsync("/api/families", familyCommand);
        var family = await familyResponse.Content.ReadFromJsonAsync<FamilyDto>();

        var response = await client.DeleteAsync($"/api/families/{family!.FamilyId}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
