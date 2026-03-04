using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Application.FamilyUsers.Queries.GetFamilyMember;
using PoopNPour.Application.UnitTests.TestHelpers;
using PoopNPour.Domain.Common.Auth;
using Xunit;

namespace PoopNPour.Application.UnitTests.FamilyUsers.Queries;

public class GetFamilyMemberQueryHandlerReturnsCorrectMember
{
    [Fact]
    public async Task Handle_ValidIds_ReturnsFamilyMember()
    {
        var familyId = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();
        var member = new FamilyMemberDtoBuilder()
            .WithFamilyId(familyId)
            .WithUserId(userId)
            .WithUserName("jsmith")
            .WithEmail("j@smith.com")
            .WithRole(FamilyRole.Admin)
            .Build();

        var familyUserService = Substitute.For<IFamilyUserService>();
        familyUserService
            .GetFamilyMemberAsync(familyId, userId, Arg.Any<CancellationToken>())
            .Returns(member);

        var sut = new GetFamilyMemberQueryHandler(familyUserService);
        var result = await sut.Handle(new GetFamilyMemberQuery(familyId, userId), CancellationToken.None);

        result.Should().BeSameAs(member);
        result.FamilyId.Should().Be(familyId);
        result.UserId.Should().Be(userId);
        result.UserName.Should().Be("jsmith");
        result.Role.Should().Be(FamilyRole.Admin);
    }
}
