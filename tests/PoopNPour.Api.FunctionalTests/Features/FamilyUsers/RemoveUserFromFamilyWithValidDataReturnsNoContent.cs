using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Abstractions.Family;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Authentication.Models;
using PoopNPour.Application.Families.Commands.CreateFamily;
using PoopNPour.Application.FamilyUsers.Commands.AddUserToFamily;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.FamilyUsers;

public class RemoveUserFromFamilyWithValidDataReturnsNoContent : AuthenticatedTestBase
{
    public RemoveUserFromFamilyWithValidDataReturnsNoContent(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task RemoveUserFromFamily_WithValidData_ReturnsNoContent()
    {
        using var client = await CreateAdminClientAsync();
        
        var familyCommand = new CreateFamilyCommand($"TestFamily-{Guid.NewGuid():N}", "LastName");
        var familyResponse = await client.PostAsJsonAsync("/api/families", familyCommand);
        var family = await familyResponse.Content.ReadFromJsonAsync<FamilyDto>();

        var webApiClient = CreateWebApiClient();
        var registerRequest = new RegisterRequestDto
        {
            Email = $"test-{Guid.NewGuid():N}@example.com",
            UserName = $"user-{Guid.NewGuid():N}",
            Password = "Password123!",
            FirstName = "Test",
            LastName = "User"
        };
        var registerResponse = await webApiClient.PostAsJsonAsync("/api/authentication/register", registerRequest);
        var registerResult = await registerResponse.Content.ReadFromJsonAsync<RegisterResponseDto>();

        var addCommand = new AddUserToFamilyCommand(family!.FamilyId, registerResult!.User.Id);
        await client.PostAsJsonAsync("/api/family-users", addCommand);

        var response = await client.DeleteAsync($"/api/family-users/{family.FamilyId}/users/{registerResult.User.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
