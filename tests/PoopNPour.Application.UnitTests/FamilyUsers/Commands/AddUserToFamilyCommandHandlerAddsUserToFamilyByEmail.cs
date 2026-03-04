using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.FamilyUsers.Commands.AddUserToFamily;
using PoopNPour.Application.UnitTests.TestHelpers;
using Xunit;

namespace PoopNPour.Application.UnitTests.FamilyUsers.Commands;

public class AddUserToFamilyCommandHandlerAddsUserToFamilyByEmail
{
    [Fact]
    public async Task Handle_ValidEmail_ResolvesUserAndAddsToFamily()
    {
        var familyId = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();
        const string email = "john.smith@example.com";

        var userDto = new UserDto { Id = userId, Email = email };
        var created = new FamilyUserDtoBuilder().WithFamilyId(familyId).WithUserId(userId).Build();

        var userService = Substitute.For<IUserService>();
        var familyUserService = Substitute.For<IFamilyUserService>();
        userService.GetUserByEmailAsync(email, Arg.Any<CancellationToken>()).Returns(userDto);
        familyUserService.AddUserToFamilyAsync(familyId, userId, Arg.Any<CancellationToken>()).Returns(created);

        var sut = new AddUserToFamilyCommandHandler(familyUserService, userService);
        var result = await sut.Handle(new AddUserToFamilyCommand(familyId, email), CancellationToken.None);

        result.Should().BeSameAs(created);
        result.FamilyId.Should().Be(familyId);
        result.UserId.Should().Be(userId);
    }
}
