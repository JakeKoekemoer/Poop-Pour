using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Application.FamilyUsers.Exceptions;
using PoopNPour.Application.FamilyUsers.Queries.GetFamilyUserById;
using PoopNPour.Application.UnitTests.TestHelpers;
using Xunit;

namespace PoopNPour.Application.UnitTests.FamilyUsers.Queries;

public class GetFamilyUserByIdQueryHandlerTests
{
    private readonly IFamilyUserService _familyUserService;
    private readonly GetFamilyUserByIdQueryHandler _sut;

    public GetFamilyUserByIdQueryHandlerTests()
    {
        _familyUserService = Substitute.For<IFamilyUserService>();
        _sut = new GetFamilyUserByIdQueryHandler(_familyUserService);
    }

    [Fact]
    public async Task Handle_ValidId_ReturnsFamilyUser()
    {
        var familyId = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();
        var familyUser = new FamilyUserDtoBuilder()
            .WithFamilyId(familyId)
            .WithUserId(userId)
            .Build();
        _familyUserService.GetFamilyUserByIdAsync(familyId, userId, Arg.Any<CancellationToken>()).Returns(familyUser);

        var query = new GetFamilyUserByIdQuery(familyId, userId);
        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().BeSameAs(familyUser);
        result.FamilyId.Should().Be(familyId);
        result.UserId.Should().Be(userId);
    }

    [Fact]
    public async Task Handle_FamilyUserNotFound_ThrowsFamilyUserNotFoundException()
    {
        var familyId = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();
        _familyUserService.GetFamilyUserByIdAsync(familyId, userId, Arg.Any<CancellationToken>()).Returns((FamilyUserDto?)null);

        var query = new GetFamilyUserByIdQuery(familyId, userId);
        var act = () => _sut.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<FamilyUserNotFoundException>();
    }
}
