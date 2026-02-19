using FluentAssertions;
using MediatR;
using NSubstitute;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Application.FamilyUsers.Commands.RemoveUserFromFamily;
using PoopNPour.Application.FamilyUsers.Exceptions;
using Xunit;

namespace PoopNPour.Application.UnitTests.FamilyUsers.Commands;

public class RemoveUserFromFamilyCommandHandlerTests
{
    private readonly IFamilyUserService _familyUserService;
    private readonly RemoveUserFromFamilyCommandHandler _sut;

    public RemoveUserFromFamilyCommandHandlerTests()
    {
        _familyUserService = Substitute.For<IFamilyUserService>();
        _sut = new RemoveUserFromFamilyCommandHandler(_familyUserService);
    }

    [Fact]
    public async Task Handle_ValidCommand_RemovesUserFromFamily()
    {
        var familyId = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();
        _familyUserService.RemoveUserFromFamilyAsync(familyId, userId, Arg.Any<CancellationToken>()).Returns(true);

        var command = new RemoveUserFromFamilyCommand(familyId, userId);
        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().Be(Unit.Value);
    }

    [Fact]
    public async Task Handle_UserNotInFamily_ThrowsFamilyUserNotFoundException()
    {
        var familyId = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();
        _familyUserService.RemoveUserFromFamilyAsync(familyId, userId, Arg.Any<CancellationToken>()).Returns(false);

        var command = new RemoveUserFromFamilyCommand(familyId, userId);
        var act = () => _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<FamilyUserNotFoundException>();
    }

    [Fact]
    public async Task Handle_CallsServiceWithCorrectParameters()
    {
        var familyId = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();
        _familyUserService.RemoveUserFromFamilyAsync(familyId, userId, Arg.Any<CancellationToken>()).Returns(true);

        var command = new RemoveUserFromFamilyCommand(familyId, userId);
        await _sut.Handle(command, CancellationToken.None);

        await _familyUserService.Received().RemoveUserFromFamilyAsync(familyId, userId, Arg.Any<CancellationToken>());
    }
}
