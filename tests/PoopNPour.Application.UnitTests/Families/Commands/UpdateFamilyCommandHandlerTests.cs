using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using PoopNPour.Abstractions.Family;
using PoopNPour.Application.Families.Commands.UpdateFamily;
using PoopNPour.Application.Families.Exceptions;
using PoopNPour.Application.UnitTests.TestHelpers;
using System.Security.Claims;
using Xunit;

namespace PoopNPour.Application.UnitTests.Families.Commands;

public class UpdateFamilyCommandHandlerTests
{
    private readonly IFamilyService _familyService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UpdateFamilyCommandHandler _sut;

    public UpdateFamilyCommandHandlerTests()
    {
        _familyService = Substitute.For<IFamilyService>();
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        
        var httpContext = new DefaultHttpContext();
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "user-123")
        }));
        _httpContextAccessor.HttpContext.Returns(httpContext);
        
        _sut = new UpdateFamilyCommandHandler(_familyService, _httpContextAccessor);
    }

    [Fact]
    public async Task Handle_UpdatesFamilyNameOnly_ReturnsUpdatedFamily()
    {
        var familyId = Guid.NewGuid();
        var updated = new FamilyDtoBuilder()
            .WithFamilyId(familyId)
            .WithFamilyName("Updated Name")
            .Build();
        _familyService.IsFamilyNameDuplicateAsync("Updated Name", "user-123", familyId, Arg.Any<CancellationToken>()).Returns(false);
        _familyService.UpdateFamilyAsync(familyId, "Updated Name", null, Arg.Any<CancellationToken>()).Returns(updated);

        var command = new UpdateFamilyCommand(familyId, "Updated Name", null);
        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().BeSameAs(updated);
        result.FamilyName.Should().Be("Updated Name");
    }

    [Fact]
    public async Task Handle_UpdatesFamilyLastNameOnly_ReturnsUpdatedFamily()
    {
        var familyId = Guid.NewGuid();
        var updated = new FamilyDtoBuilder()
            .WithFamilyId(familyId)
            .WithFamilyLastName("Updated LastName")
            .Build();
        _familyService.UpdateFamilyAsync(familyId, null, "Updated LastName", Arg.Any<CancellationToken>()).Returns(updated);

        var command = new UpdateFamilyCommand(familyId, null, "Updated LastName");
        var result = await _sut.Handle(command, CancellationToken.None);

        result.FamilyLastName.Should().Be("Updated LastName");
    }

    [Fact]
    public async Task Handle_UpdatesBothFields_ReturnsUpdatedFamily()
    {
        var familyId = Guid.NewGuid();
        var updated = new FamilyDtoBuilder()
            .WithFamilyId(familyId)
            .WithFamilyName("New Name")
            .WithFamilyLastName("New LastName")
            .Build();
        _familyService.IsFamilyNameDuplicateAsync("New Name", "user-123", familyId, Arg.Any<CancellationToken>()).Returns(false);
        _familyService.UpdateFamilyAsync(familyId, "New Name", "New LastName", Arg.Any<CancellationToken>()).Returns(updated);

        var command = new UpdateFamilyCommand(familyId, "New Name", "New LastName");
        var result = await _sut.Handle(command, CancellationToken.None);

        result.FamilyName.Should().Be("New Name");
        result.FamilyLastName.Should().Be("New LastName");
    }

    [Fact]
    public async Task Handle_DuplicateFamilyName_ThrowsDuplicateFamilyNameException()
    {
        var familyId = Guid.NewGuid();
        _familyService.IsFamilyNameDuplicateAsync("Duplicate", "user-123", familyId, Arg.Any<CancellationToken>()).Returns(true);

        var command = new UpdateFamilyCommand(familyId, "Duplicate", null);
        var act = () => _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<DuplicateFamilyNameException>().WithMessage("*Duplicate*");
    }

    [Fact]
    public async Task Handle_FamilyNotFound_ThrowsFamilyNotFoundException()
    {
        var familyId = Guid.NewGuid();
        _familyService.UpdateFamilyAsync(familyId, null, "LastName", Arg.Any<CancellationToken>()).Returns((FamilyDto?)null);

        var command = new UpdateFamilyCommand(familyId, null, "LastName");
        var act = () => _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<FamilyNotFoundException>();
    }

    [Fact]
    public async Task Handle_AllNullFields_SkipsDuplicateCheck()
    {
        var familyId = Guid.NewGuid();
        var updated = new FamilyDtoBuilder().WithFamilyId(familyId).Build();
        _familyService.UpdateFamilyAsync(familyId, null, null, Arg.Any<CancellationToken>()).Returns(updated);

        var command = new UpdateFamilyCommand(familyId, null, null);
        await _sut.Handle(command, CancellationToken.None);

        await _familyService.DidNotReceive().IsFamilyNameDuplicateAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>());
    }
}
