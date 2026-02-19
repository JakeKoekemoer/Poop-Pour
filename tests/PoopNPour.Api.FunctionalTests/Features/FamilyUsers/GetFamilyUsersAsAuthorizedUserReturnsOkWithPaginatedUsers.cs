using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Api.FunctionalTests.Infrastructure;
using PoopNPour.Application.Common.Models;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Features.FamilyUsers;

public class GetFamilyUsersAsAuthorizedUserReturnsOkWithPaginatedUsers : AuthenticatedTestBase
{
    public GetFamilyUsersAsAuthorizedUserReturnsOkWithPaginatedUsers(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task GetFamilyUsers_AsAuthorizedUser_ReturnsOkWithPaginatedUsers()
    {
        using var client = await CreateAdminClientAsync();

        var response = await client.GetAsync("/api/family-users?page=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PaginatedResponseDto<FamilyUserDto>>();
        result.Should().NotBeNull();
        result!.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
    }
}
