using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Abstractions.Family;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Families.Commands.CreateFamily;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Families;

public class CreateFamilyWithValidDataReturnsCreatedWithFamily : AuthenticatedTestBase
{
    public CreateFamilyWithValidDataReturnsCreatedWithFamily(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task CreateFamily_WithValidData_ReturnsCreatedWithFamily()
    {
        using var client = await CreateAdminClientAsync();
        var command = new CreateFamilyCommand($"TestFamily-{Guid.NewGuid():N}", "TestLastName");

        var response = await client.PostAsJsonAsync("/api/families", command);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<FamilyDto>();
        result.Should().NotBeNull();
        result!.FamilyName.Should().Be(command.FamilyName);
        result.FamilyLastName.Should().Be(command.FamilyLastName);
        result.FamilyId.Should().NotBeEmpty();
    }
}
