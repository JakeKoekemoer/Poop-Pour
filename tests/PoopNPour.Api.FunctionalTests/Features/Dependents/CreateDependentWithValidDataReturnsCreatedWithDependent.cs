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

public class CreateDependentWithValidDataReturnsCreatedWithDependent : AuthenticatedTestBase
{
    public CreateDependentWithValidDataReturnsCreatedWithDependent(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task CreateDependent_WithValidData_ReturnsCreatedWithDependent()
    {
        using var client = await CreateAdminClientAsync();
        
        var familyCommand = new CreateFamilyCommand($"Family-{Guid.NewGuid():N}", "LastName");
        var familyResponse = await client.PostAsJsonAsync("/api/families", familyCommand);
        var family = await familyResponse.Content.ReadFromJsonAsync<FamilyDto>();

        var command = new CreateDependentCommand(family!.FamilyId, "Child", "Doe", DateTimeOffset.UtcNow.AddYears(-2));
        var response = await client.PostAsJsonAsync("/api/dependents", command);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<DependentDto>();
        result.Should().NotBeNull();
        result!.DependentName.Should().Be("Child");
        result.DependentSurname.Should().Be("Doe");
    }
}
