using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using PoopNPour.Abstractions.Family;
using PoopNPour.Application.Families.Commands.CreateFamily;
using PoopNPour.Application.Families.Exceptions;
using PoopNPour.Application.UnitTests.TestHelpers;
using System.Security.Claims;
using Xunit;

namespace PoopNPour.Application.UnitTests.Families.Commands;

public class CreateFamilyCommandHandlerTests
{
    private readonly IFamilyService _familyService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly CreateFamilyCommandHandler _sut;

    public CreateFamilyCommandHandlerTests()
    {
        _familyService = Substitute.For<IFamilyService>();
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        
        var httpContext = new DefaultHttpContext();
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "user-123")
        }));
        _httpContextAccessor.HttpContext.Returns(httpContext);
        
        _sut = new CreateFamilyCommandHandler(_familyService, _httpContextAccessor);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesFamilySuccessfully()
    {
        var created = new FamilyDtoBuilder()
            .WithFamilyName("Smith Family")
            .WithFamilyLastName("Smith")
            .Build();
        _familyService.IsFamilyNameDuplicateAsync("Smith Family", "user-123", null, Arg.Any<CancellationToken>()).Returns(false);
        _familyService.CreateFamilyAsync("Smith Family", "Smith", "user-123", Arg.Any<CancellationToken>()).Returns(created);

        var command = new CreateFamilyCommand("Smith Family", "Smith");
        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().BeSameAs(created);
        result.FamilyName.Should().Be("Smith Family");
        result.FamilyLastName.Should().Be("Smith");
    }

    [Fact]
    public async Task Handle_DuplicateFamilyName_ThrowsDuplicateFamilyNameException()
    {
        _familyService.IsFamilyNameDuplicateAsync("Duplicate", "user-123", null, Arg.Any<CancellationToken>()).Returns(true);

        var command = new CreateFamilyCommand("Duplicate", "Smith");
        var act = () => _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<DuplicateFamilyNameException>().WithMessage("*Duplicate*");
    }

    [Fact]
    public async Task Handle_CallsServiceWithCorrectParameters()
    {
        var created = new FamilyDtoBuilder().Build();
        _familyService.IsFamilyNameDuplicateAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>()).Returns(false);
        _familyService.CreateFamilyAsync("TestFamily", "TestLastName", "user-123", Arg.Any<CancellationToken>()).Returns(created);

        var command = new CreateFamilyCommand("TestFamily", "TestLastName");
        await _sut.Handle(command, CancellationToken.None);

        await _familyService.Received().IsFamilyNameDuplicateAsync("TestFamily", "user-123", null, Arg.Any<CancellationToken>());
        await _familyService.Received().CreateFamilyAsync("TestFamily", "TestLastName", "user-123", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ServiceReturnsFamily_ReturnsCreatedFamily()
    {
        var familyId = Guid.NewGuid();
        var created = new FamilyDtoBuilder()
            .WithFamilyId(familyId)
            .WithFamilyName("New Family")
            .WithFamilyLastName("NewLast")
            .Build();
        _familyService.IsFamilyNameDuplicateAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>()).Returns(false);
        _familyService.CreateFamilyAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<CancellationToken>()).Returns(created);

        var command = new CreateFamilyCommand("New Family", "NewLast");
        var result = await _sut.Handle(command, CancellationToken.None);

        result.FamilyId.Should().Be(familyId);
    }

    [Fact]
    public async Task Handle_ChecksDuplicateBeforeCreating()
    {
        var created = new FamilyDtoBuilder().Build();
        _familyService.IsFamilyNameDuplicateAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>()).Returns(false);
        _familyService.CreateFamilyAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<CancellationToken>()).Returns(created);

        var command = new CreateFamilyCommand("Family", "Name");
        await _sut.Handle(command, CancellationToken.None);

        Received.InOrder(() =>
        {
            _familyService.IsFamilyNameDuplicateAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>());
            _familyService.CreateFamilyAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<CancellationToken>());
        });
    }
}
