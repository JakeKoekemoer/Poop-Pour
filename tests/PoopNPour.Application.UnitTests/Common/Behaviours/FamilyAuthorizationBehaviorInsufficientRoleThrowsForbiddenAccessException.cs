using FluentAssertions;
using MediatR;
using NSubstitute;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Common.Behaviours;
using PoopNPour.Application.Common.Exceptions;
using PoopNPour.Application.Common.Interfaces;
using PoopNPour.Application.UnitTests.TestHelpers;
using PoopNPour.Domain.Common.Auth;
using Xunit;

namespace PoopNPour.Application.UnitTests.Common.Behaviours;

// Requires Admin but user only has Member
[AuthorizeFamilyMember(FamilyRole.Admin)]
public record InsufficientRoleRequest(Guid FamilyId) : IRequest<string>, IFamilyRequest;

public class FamilyAuthorizationBehaviorInsufficientRoleThrowsForbiddenAccessException
{
    [Fact]
    public async Task Handle_UserRoleBelowMinimum_ThrowsForbiddenAccessException()
    {
        var familyId = Guid.NewGuid();
        const string userId = "user-1";

        var user = CurrentUserBuilder.CreateAuthenticated(userId)
            .WithFamilyIds([familyId]);

        var familyUserDto = new FamilyUserDtoBuilder()
            .WithFamilyId(familyId)
            .WithUserId(userId)
            .WithRole(FamilyRole.Member) // Below required Admin
            .Build();

        var familyUserService = Substitute.For<IFamilyUserService>();
        familyUserService
            .GetFamilyUserByIdAsync(familyId, userId, Arg.Any<CancellationToken>())
            .Returns(familyUserDto);

        var behavior = new FamilyAuthorizationBehavior<InsufficientRoleRequest, string>(user, familyUserService);
        var request = new InsufficientRoleRequest(familyId);

        var act = () => behavior.Handle(request, _ => Task.FromResult("result"), CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenAccessException>()
            .WithMessage("*Member*")
            .WithMessage("*Admin*");
    }
}
