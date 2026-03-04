using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.FamilyUsers.Commands.AddUserToFamily;
using PoopNPour.Application.Users.Exceptions;
using Xunit;

namespace PoopNPour.Application.UnitTests.FamilyUsers.Commands;

public class AddUserToFamilyCommandHandlerUserNotFoundByEmailThrowsUserNotFoundException
{
    [Fact]
    public async Task Handle_EmailNotFound_ThrowsUserNotFoundException()
    {
        const string email = "nobody@example.com";

        var userService = Substitute.For<IUserService>();
        var familyUserService = Substitute.For<IFamilyUserService>();
        userService.GetUserByEmailAsync(email, Arg.Any<CancellationToken>()).Returns((UserDto?)null);

        var sut = new AddUserToFamilyCommandHandler(familyUserService, userService);
        var act = () => sut.Handle(new AddUserToFamilyCommand(Guid.NewGuid(), email), CancellationToken.None);

        await act.Should().ThrowAsync<UserNotFoundException>();
        await familyUserService.DidNotReceive().AddUserToFamilyAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }
}
