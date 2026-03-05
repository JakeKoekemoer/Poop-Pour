using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Abstractions.Dependent;
using PoopNPour.Abstractions.Family;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Dependents.Commands.CreateDependent;
using PoopNPour.Application.Families.Commands.CreateFamily;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Dependents;

public class DeleteDependentWithValidDataReturnsNoContent : AuthenticatedTestBase
{
    public DeleteDependentWithValidDataReturnsNoContent(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task DeleteDependent_WithValidData_ReturnsNoContent()
    {
        using var client = await CreateAdminClientAsync();

        var familyCommand = new CreateFamilyCommand($"Family-{Guid.NewGuid():N}", "LastName");
        var familyResponse = await client.PostAsJsonAsync("/api/families", familyCommand);
        var family = await familyResponse.Content.ReadFromJsonAsync<FamilyDto>();

        var dependentCommand = new CreateDependentCommand(family!.FamilyId, "Child", "Doe", DateTimeOffset.UtcNow.AddYears(-2));
        var dependentResponse = await client.PostAsJsonAsync("/api/dependents", dependentCommand);
        var dependent = await dependentResponse.Content.ReadFromJsonAsync<DependentDto>();

        var response = await client.DeleteAsync($"/api/dependents/{dependent!.DependentId}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
