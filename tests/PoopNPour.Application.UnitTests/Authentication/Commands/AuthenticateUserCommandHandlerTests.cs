using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.Authentication;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.Authentication.Commands;
using PoopNPour.Application.Authentication.Exceptions;
using PoopNPour.Application.UnitTests.TestHelpers;
using Xunit;

namespace PoopNPour.Application.UnitTests.Authentication.Commands;

public class AuthenticateUserCommandHandlerTests
{
    private readonly IUserService _userService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IFamilyUserService _familyUserService;
    private readonly AuthenticateUserCommandHandler _sut;

    public AuthenticateUserCommandHandlerTests()
    {
        _userService = Substitute.For<IUserService>();
        _jwtTokenService = Substitute.For<IJwtTokenService>();
        _familyUserService = Substitute.For<IFamilyUserService>();
        _familyUserService
            .GetUserFamilyMembershipsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns([]);
        _sut = new AuthenticateUserCommandHandler(_userService, _jwtTokenService, _familyUserService);
    }

    [Fact]
    public async Task Handle_ValidCredentials_ReturnsLoginResponseDtoWithToken()
    {
        var user = new UserDtoBuilder().WithUserName("admin").WithEmail("admin@test.com").Build();
        _userService
            .ValidatePasswordAndGetUserAsync("admin", "password", Arg.Any<CancellationToken>())
            .Returns(user);
        _jwtTokenService
            .GenerateTokenAsync(user.Id, user.Email, user.UserName, user.Roles, Arg.Any<IEnumerable<FamilyUserDto>?>())
            .Returns(("jwt-token", new DateTime(2025, 12, 31)));

        var command = new AuthenticateUserCommand("admin", "password");
        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Token.Should().Be("jwt-token");
        result.ExpiresAt.Should().Be(new DateTime(2025, 12, 31));
        result.User.Should().BeSameAs(user);
    }

    [Fact]
    public async Task Handle_ValidCredentials_FetchesFamilyMembershipsAndPassesToTokenService()
    {
        var user = new UserDtoBuilder().Build();
        var memberships = new[]
        {
            new FamilyUserDtoBuilder().WithUserId(user.Id).Build(),
            new FamilyUserDtoBuilder().WithUserId(user.Id).Build()
        };
        _userService
            .ValidatePasswordAndGetUserAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(user);
        _familyUserService
            .GetUserFamilyMembershipsAsync(user.Id, Arg.Any<CancellationToken>())
            .Returns(memberships);
        _jwtTokenService
            .GenerateTokenAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IEnumerable<string>>(), Arg.Any<IEnumerable<FamilyUserDto>?>())
            .Returns(("jwt-token", DateTime.UtcNow));

        await _sut.Handle(new AuthenticateUserCommand("user", "pass"), CancellationToken.None);

        await _familyUserService.Received(1).GetUserFamilyMembershipsAsync(user.Id, Arg.Any<CancellationToken>());
        await _jwtTokenService.Received(1).GenerateTokenAsync(
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IEnumerable<string>>(),
            Arg.Is<IEnumerable<FamilyUserDto>?>(m => m != null && m.Count() == 2));
    }

    [Fact]
    public async Task Handle_InvalidPassword_ThrowsInvalidCredentialsException()
    {
        _userService
            .ValidatePasswordAndGetUserAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((UserDto?)null);

        var command = new AuthenticateUserCommand("admin", "wrong");
        var act = () => _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidCredentialsException>();
    }

    [Fact]
    public async Task Handle_NonExistentUser_ThrowsInvalidCredentialsException()
    {
        _userService
            .ValidatePasswordAndGetUserAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((UserDto?)null);

        var command = new AuthenticateUserCommand("nonexistent", "password");
        var act = () => _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidCredentialsException>();
    }
}
