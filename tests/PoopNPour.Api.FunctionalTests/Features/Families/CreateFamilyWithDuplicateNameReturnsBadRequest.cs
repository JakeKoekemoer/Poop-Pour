using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Families.Commands.CreateFamily;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.Families;

public class CreateFamilyWithDuplicateNameReturnsBadRequest : AuthenticatedTestBase
{
    public CreateFamilyWithDuplicateNameReturnsBadRequest(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task CreateFamily_WithDuplicateName_ReturnsBadRequest()
    {
        using var client = await CreateAdminClientAsync();
        var familyName = $"DuplicateFamily-{Guid.NewGuid():N}";
        var command1 = new CreateFamilyCommand(familyName, "LastName");
        var command2 = new CreateFamilyCommand(familyName, "LastName");

        await client.PostAsJsonAsync("/api/families", command1);
        var response = await client.PostAsJsonAsync("/api/families", command2);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
