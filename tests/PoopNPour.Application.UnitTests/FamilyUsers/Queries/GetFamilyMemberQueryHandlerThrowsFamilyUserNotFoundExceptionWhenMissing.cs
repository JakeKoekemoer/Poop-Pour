using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Application.FamilyUsers.Exceptions;
using PoopNPour.Application.FamilyUsers.Queries.GetFamilyMember;
using Xunit;

namespace PoopNPour.Application.UnitTests.FamilyUsers.Queries;

public class GetFamilyMemberQueryHandlerThrowsFamilyUserNotFoundExceptionWhenMissing
{
    [Fact]
    public async Task Handle_MemberNotFound_ThrowsFamilyUserNotFoundException()
    {
        var familyId = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();

        var familyUserService = Substitute.For<IFamilyUserService>();
        familyUserService
            .GetFamilyMemberAsync(familyId, userId, Arg.Any<CancellationToken>())
            .Returns((FamilyMemberDto?)null);

        var sut = new GetFamilyMemberQueryHandler(familyUserService);
        var act = () => sut.Handle(new GetFamilyMemberQuery(familyId, userId), CancellationToken.None);

        await act.Should().ThrowAsync<FamilyUserNotFoundException>();
    }
}
