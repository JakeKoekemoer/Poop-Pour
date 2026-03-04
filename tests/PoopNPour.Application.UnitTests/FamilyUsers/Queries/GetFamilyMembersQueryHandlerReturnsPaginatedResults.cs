using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Application.FamilyUsers.Queries.GetFamilyMembers;
using PoopNPour.Application.UnitTests.TestHelpers;
using Xunit;

namespace PoopNPour.Application.UnitTests.FamilyUsers.Queries;

public class GetFamilyMembersQueryHandlerReturnsPaginatedResults
{
    [Fact]
    public async Task Handle_ReturnsPaginatedMemberResults()
    {
        var familyId = Guid.NewGuid();
        var members = new[]
        {
            new FamilyMemberDtoBuilder().WithFamilyId(familyId).Build(),
            new FamilyMemberDtoBuilder().WithFamilyId(familyId).Build()
        };

        var familyUserService = Substitute.For<IFamilyUserService>();
        familyUserService
            .GetFamilyMembersAsync(familyId, 1, 20, Arg.Any<CancellationToken>())
            .Returns((members, 2));

        var sut = new GetFamilyMembersQueryHandler(familyUserService);
        var result = await sut.Handle(new GetFamilyMembersQuery(familyId, 1, 20), CancellationToken.None);

        result.Items.Should().HaveCount(2);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(20);
        result.TotalCount.Should().Be(2);
    }
}
