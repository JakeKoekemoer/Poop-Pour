using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.FamilyUsers.Commands.AddUserToFamily;
using PoopNPour.Application.FamilyUsers.Exceptions;
using Xunit;

namespace PoopNPour.Application.UnitTests.FamilyUsers.Commands;

public class AddUserToFamilyCommandHandlerUserAlreadyInFamilyThrowsException
{
    [Fact]
    public async Task Handle_UserAlreadyInFamily_ThrowsUserAlreadyInFamilyException()
    {
        var familyId = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();
        const string email = "member@example.com";

        var userDto = new UserDto { Id = userId, Email = email };

        var userService = Substitute.For<IUserService>();
        var familyUserService = Substitute.For<IFamilyUserService>();
        userService.GetUserByEmailAsync(email, Arg.Any<CancellationToken>()).Returns(userDto);
        familyUserService
            .AddUserToFamilyAsync(familyId, userId, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<FamilyUserDto>(new UserAlreadyInFamilyException(familyId, userId)));

        var sut = new AddUserToFamilyCommandHandler(familyUserService, userService);
        var act = () => sut.Handle(new AddUserToFamilyCommand(familyId, email), CancellationToken.None);

        await act.Should().ThrowAsync<UserAlreadyInFamilyException>();
    }
}
