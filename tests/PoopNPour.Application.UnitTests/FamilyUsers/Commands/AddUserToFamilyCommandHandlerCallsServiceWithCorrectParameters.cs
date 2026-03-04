using NSubstitute;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.FamilyUsers.Commands.AddUserToFamily;
using PoopNPour.Application.UnitTests.TestHelpers;
using Xunit;

namespace PoopNPour.Application.UnitTests.FamilyUsers.Commands;

public class AddUserToFamilyCommandHandlerCallsServiceWithCorrectParameters
{
    [Fact]
    public async Task Handle_CallsFamilyServiceWithResolvedUserId()
    {
        var familyId = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();
        const string email = "john.smith@example.com";

        var userDto = new UserDto { Id = userId, Email = email };

        var userService = Substitute.For<IUserService>();
        var familyUserService = Substitute.For<IFamilyUserService>();
        userService.GetUserByEmailAsync(email, Arg.Any<CancellationToken>()).Returns(userDto);
        familyUserService.AddUserToFamilyAsync(familyId, userId, Arg.Any<CancellationToken>())
            .Returns(new FamilyUserDtoBuilder().Build());

        var sut = new AddUserToFamilyCommandHandler(familyUserService, userService);
        await sut.Handle(new AddUserToFamilyCommand(familyId, email), CancellationToken.None);

        await userService.Received(1).GetUserByEmailAsync(email, Arg.Any<CancellationToken>());
        await familyUserService.Received(1).AddUserToFamilyAsync(familyId, userId, Arg.Any<CancellationToken>());
    }
}
