using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Abstractions.Family;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Families.Commands.CreateFamily;
using PoopNPour.Application.Families.Commands.UpdateFamily;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Families;

public class UpdateFamilyWithValidDataReturnsOk : AuthenticatedTestBase
{
    public UpdateFamilyWithValidDataReturnsOk(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task UpdateFamily_WithValidData_ReturnsOk()
    {
        using var client = await CreateAdminClientAsync();
        var createCommand = new CreateFamilyCommand($"Original-{Guid.NewGuid():N}", "OriginalLast");
        var createResponse = await client.PostAsJsonAsync("/api/families", createCommand);
        var created = await createResponse.Content.ReadFromJsonAsync<FamilyDto>();

        var updateCommand = new UpdateFamilyCommand(created!.FamilyId, "UpdatedName", null);
        var response = await client.PutAsJsonAsync($"/api/families/{created.FamilyId}", updateCommand);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<FamilyDto>();
        result!.FamilyName.Should().Be("UpdatedName");
    }
}
