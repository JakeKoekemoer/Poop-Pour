using FluentAssertions;
using MediatR;
using NSubstitute;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Common.Behaviours;
using PoopNPour.Application.Common.Exceptions;
using PoopNPour.Application.UnitTests.TestHelpers;
using Xunit;

namespace PoopNPour.Application.UnitTests.Common.Behaviours;

// Request types with different Authorize configurations for testing
public record RequestWithoutAuthorize : IRequest<string>;

[Authorize(Roles = "Admin")]
public record RequestWithSingleRole : IRequest<string>;

[Authorize(Roles = "Admin,Manager")]
public record RequestWithMultipleRolesOr : IRequest<string>;

[Authorize(Roles = new[] { "Admin", "Manager" }, RequireAllRoles = true)]
public record RequestWithMultipleRolesAnd : IRequest<string>;

[Authorize(Policy = "CanViewUsers")]
public record RequestWithSinglePolicy : IRequest<string>;

[Authorize(Policies = new[] { "CanViewUsers", "CanEditUsers" })]
public record RequestWithMultiplePoliciesOr : IRequest<string>;

[Authorize(Policies = new[] { "CanViewUsers", "CanEditUsers" }, RequireAllPolicies = true)]
public record RequestWithMultiplePoliciesAnd : IRequest<string>;

[Authorize(Roles = "Admin", Policy = "CanViewUsers")]
public record RequestWithRoleAndPolicy : IRequest<string>;

public class AuthorizationBehaviorTests
{
    private readonly IUser _user;
    private readonly IIdentityService _identityService;
    private static RequestHandlerDelegate<string> Next => (ct) => Task.FromResult("result");

    public AuthorizationBehaviorTests()
    {
        _user = Substitute.For<IUser>();
        _identityService = Substitute.For<IIdentityService>();
    }

    [Fact]
    public async Task Handle_RequestWithoutAuthorize_ProceedsWithoutChecks()
    {
        var behavior = new AuthorizationBehavior<RequestWithoutAuthorize, string>(_user, _identityService);
        var request = new RequestWithoutAuthorize();

        var result = await behavior.Handle(request, Next, CancellationToken.None);

        result.Should().Be("result");
        await _identityService.DidNotReceive().IsInRoleAsync(Arg.Any<string>(), Arg.Any<string>());
        await _identityService.DidNotReceive().AuthorizeAsync(Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public async Task Handle_UnauthenticatedUser_ThrowsUnauthorizedAccessException()
    {
        var unauthenticatedUser = CurrentUserBuilder.CreateUnauthenticated();
        var behavior = new AuthorizationBehavior<RequestWithSingleRole, string>(unauthenticatedUser, _identityService);
        var request = new RequestWithSingleRole();

        var act = () => behavior.Handle(request, Next, CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedAccessException>().WithMessage("*not authenticated*");
        await _identityService.DidNotReceive().IsInRoleAsync(Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public async Task Handle_AuthenticatedUserWithNullId_ThrowsUnauthorizedAccessException()
    {
        var user = Substitute.For<IUser>();
        user.IsAuthenticated.Returns(true);
        user.Id.Returns((string?)null);
        var behavior = new AuthorizationBehavior<RequestWithSingleRole, string>(user, _identityService);
        var request = new RequestWithSingleRole();

        var act = () => behavior.Handle(request, Next, CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task Handle_SingleRole_UserHasRole_Proceeds()
    {
        _user.Id.Returns("user-1");
        _user.IsAuthenticated.Returns(true);
        _identityService.IsInRoleAsync("user-1", "Admin").Returns(true);
        var behavior = new AuthorizationBehavior<RequestWithSingleRole, string>(_user, _identityService);
        var request = new RequestWithSingleRole();

        var result = await behavior.Handle(request, Next, CancellationToken.None);

        result.Should().Be("result");
        await _identityService.Received().IsInRoleAsync("user-1", "Admin");
    }

    [Fact]
    public async Task Handle_SingleRole_UserDoesNotHaveRole_ThrowsForbiddenAccessException()
    {
        _user.Id.Returns("user-1");
        _user.IsAuthenticated.Returns(true);
        _identityService.IsInRoleAsync("user-1", "Admin").Returns(false);
        var behavior = new AuthorizationBehavior<RequestWithSingleRole, string>(_user, _identityService);
        var request = new RequestWithSingleRole();

        var act = () => behavior.Handle(request, Next, CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenAccessException>().WithMessage("*Admin*");
    }

    [Fact]
    public async Task Handle_MultipleRolesOr_UserHasOneRole_Proceeds()
    {
        _user.Id.Returns("user-1");
        _user.IsAuthenticated.Returns(true);
        _identityService.IsInRoleAsync("user-1", "Admin").Returns(false);
        _identityService.IsInRoleAsync("user-1", "Manager").Returns(true);
        var behavior = new AuthorizationBehavior<RequestWithMultipleRolesOr, string>(_user, _identityService);
        var request = new RequestWithMultipleRolesOr();

        var result = await behavior.Handle(request, Next, CancellationToken.None);

        result.Should().Be("result");
    }

    [Fact]
    public async Task Handle_MultipleRolesRequireAll_UserHasAll_Proceeds()
    {
        _user.Id.Returns("user-1");
        _user.IsAuthenticated.Returns(true);
        _identityService.IsInRoleAsync("user-1", "Admin").Returns(true);
        _identityService.IsInRoleAsync("user-1", "Manager").Returns(true);
        var behavior = new AuthorizationBehavior<RequestWithMultipleRolesAnd, string>(_user, _identityService);
        var request = new RequestWithMultipleRolesAnd();

        var result = await behavior.Handle(request, Next, CancellationToken.None);

        result.Should().Be("result");
    }

    [Fact]
    public async Task Handle_MultipleRolesRequireAll_UserMissingOne_ThrowsForbiddenAccessException()
    {
        _user.Id.Returns("user-1");
        _user.IsAuthenticated.Returns(true);
        _identityService.IsInRoleAsync("user-1", "Admin").Returns(true);
        _identityService.IsInRoleAsync("user-1", "Manager").Returns(false);
        var behavior = new AuthorizationBehavior<RequestWithMultipleRolesAnd, string>(_user, _identityService);
        var request = new RequestWithMultipleRolesAnd();

        var act = () => behavior.Handle(request, Next, CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Fact]
    public async Task Handle_SinglePolicy_UserSatisfiesPolicy_Proceeds()
    {
        _user.Id.Returns("user-1");
        _user.IsAuthenticated.Returns(true);
        _identityService.AuthorizeAsync("user-1", "CanViewUsers").Returns(true);
        var behavior = new AuthorizationBehavior<RequestWithSinglePolicy, string>(_user, _identityService);
        var request = new RequestWithSinglePolicy();

        var result = await behavior.Handle(request, Next, CancellationToken.None);

        result.Should().Be("result");
        await _identityService.Received().AuthorizeAsync("user-1", "CanViewUsers");
    }

    [Fact]
    public async Task Handle_SinglePolicy_UserDoesNotSatisfy_ThrowsForbiddenAccessException()
    {
        _user.Id.Returns("user-1");
        _user.IsAuthenticated.Returns(true);
        _identityService.AuthorizeAsync("user-1", "CanViewUsers").Returns(false);
        var behavior = new AuthorizationBehavior<RequestWithSinglePolicy, string>(_user, _identityService);
        var request = new RequestWithSinglePolicy();

        var act = () => behavior.Handle(request, Next, CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenAccessException>().WithMessage("*CanViewUsers*");
    }

    [Fact]
    public async Task Handle_MultiplePoliciesOr_UserSatisfiesOne_Proceeds()
    {
        _user.Id.Returns("user-1");
        _user.IsAuthenticated.Returns(true);
        _identityService.AuthorizeAsync("user-1", "CanViewUsers").Returns(false);
        _identityService.AuthorizeAsync("user-1", "CanEditUsers").Returns(true);
        var behavior = new AuthorizationBehavior<RequestWithMultiplePoliciesOr, string>(_user, _identityService);
        var request = new RequestWithMultiplePoliciesOr();

        var result = await behavior.Handle(request, Next, CancellationToken.None);

        result.Should().Be("result");
    }

    [Fact]
    public async Task Handle_MultiplePoliciesRequireAll_UserSatisfiesAll_Proceeds()
    {
        _user.Id.Returns("user-1");
        _user.IsAuthenticated.Returns(true);
        _identityService.AuthorizeAsync("user-1", "CanViewUsers").Returns(true);
        _identityService.AuthorizeAsync("user-1", "CanEditUsers").Returns(true);
        var behavior = new AuthorizationBehavior<RequestWithMultiplePoliciesAnd, string>(_user, _identityService);
        var request = new RequestWithMultiplePoliciesAnd();

        var result = await behavior.Handle(request, Next, CancellationToken.None);

        result.Should().Be("result");
    }

    [Fact]
    public async Task Handle_MultiplePoliciesRequireAll_UserMissingOne_ThrowsForbiddenAccessException()
    {
        _user.Id.Returns("user-1");
        _user.IsAuthenticated.Returns(true);
        _identityService.AuthorizeAsync("user-1", "CanViewUsers").Returns(true);
        _identityService.AuthorizeAsync("user-1", "CanEditUsers").Returns(false);
        var behavior = new AuthorizationBehavior<RequestWithMultiplePoliciesAnd, string>(_user, _identityService);
        var request = new RequestWithMultiplePoliciesAnd();

        var act = () => behavior.Handle(request, Next, CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Fact]
    public async Task Handle_CombinedRoleAndPolicy_BothSatisfied_Proceeds()
    {
        _user.Id.Returns("user-1");
        _user.IsAuthenticated.Returns(true);
        _identityService.IsInRoleAsync("user-1", "Admin").Returns(true);
        _identityService.AuthorizeAsync("user-1", "CanViewUsers").Returns(true);
        var behavior = new AuthorizationBehavior<RequestWithRoleAndPolicy, string>(_user, _identityService);
        var request = new RequestWithRoleAndPolicy();

        var result = await behavior.Handle(request, Next, CancellationToken.None);

        result.Should().Be("result");
    }
}
