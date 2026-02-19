using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Application.FamilyUsers.Commands.AddUserToFamily;
using PoopNPour.Application.FamilyUsers.Exceptions;
using PoopNPour.Application.UnitTests.TestHelpers;
using Xunit;

namespace PoopNPour.Application.UnitTests.FamilyUsers.Commands;

public class AddUserToFamilyCommandHandlerTests
{
    private readonly IFamilyUserService _familyUserService;
    private readonly AddUserToFamilyCommandHandler _sut;

    public AddUserToFamilyCommandHandlerTests()
    {
        _familyUserService = Substitute.For<IFamilyUserService>();
        _sut = new AddUserToFamilyCommandHandler(_familyUserService);
    }

    [Fact]
    public async Task Handle_ValidCommand_AddsUserToFamily()
    {
        var familyId = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();
        var created = new FamilyUserDtoBuilder()
            .WithFamilyId(familyId)
            .WithUserId(userId)
            .Build();
        _familyUserService.AddUserToFamilyAsync(familyId, userId, Arg.Any<CancellationToken>()).Returns(created);

        var command = new AddUserToFamilyCommand(familyId, userId);
        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().BeSameAs(created);
        result.FamilyId.Should().Be(familyId);
        result.UserId.Should().Be(userId);
    }

    [Fact]
    public async Task Handle_UserAlreadyInFamily_ThrowsUserAlreadyInFamilyException()
    {
        var familyId = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();
        _familyUserService
            .AddUserToFamilyAsync(familyId, userId, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<FamilyUserDto>(new UserAlreadyInFamilyException(familyId, userId)));

        var command = new AddUserToFamilyCommand(familyId, userId);
        var act = () => _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<UserAlreadyInFamilyException>();
    }

    [Fact]
    public async Task Handle_CallsServiceWithCorrectParameters()
    {
        var familyId = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();
        var created = new FamilyUserDtoBuilder().Build();
        _familyUserService.AddUserToFamilyAsync(familyId, userId, Arg.Any<CancellationToken>()).Returns(created);

        var command = new AddUserToFamilyCommand(familyId, userId);
        await _sut.Handle(command, CancellationToken.None);

        await _familyUserService.Received().AddUserToFamilyAsync(familyId, userId, Arg.Any<CancellationToken>());
    }
}
