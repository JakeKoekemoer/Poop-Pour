using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Application.FamilyUsers.Queries.GetFamilyMembers;
using Xunit;

namespace PoopNPour.Application.UnitTests.FamilyUsers.Queries;

public class GetFamilyMembersQueryHandlerReturnsEmptyWhenPageBeyondTotal
{
    [Fact]
    public async Task Handle_PageBeyondTotal_ReturnsEmptyItemsWithCorrectTotalCount()
    {
        var familyId = Guid.NewGuid();

        var familyUserService = Substitute.For<IFamilyUserService>();
        familyUserService
            .GetFamilyMembersAsync(familyId, 99, 20, Arg.Any<CancellationToken>())
            .Returns((Array.Empty<FamilyMemberDto>(), 3));

        var sut = new GetFamilyMembersQueryHandler(familyUserService);
        var result = await sut.Handle(new GetFamilyMembersQuery(familyId, 99, 20), CancellationToken.None);

        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(3);
        result.PageNumber.Should().Be(99);
    }
}
