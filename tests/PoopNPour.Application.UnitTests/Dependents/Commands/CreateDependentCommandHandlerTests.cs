using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.Dependent;
using PoopNPour.Application.Dependents.Commands.CreateDependent;
using PoopNPour.Application.Dependents.Exceptions;
using PoopNPour.Application.UnitTests.TestHelpers;
using Xunit;

namespace PoopNPour.Application.UnitTests.Dependents.Commands;

public class CreateDependentCommandHandlerTests
{
    private readonly IDependentService _dependentService;
    private readonly CreateDependentCommandHandler _sut;

    public CreateDependentCommandHandlerTests()
    {
        _dependentService = Substitute.For<IDependentService>();
        _sut = new CreateDependentCommandHandler(_dependentService);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesDependentSuccessfully()
    {
        var familyId = Guid.NewGuid();
        var created = new DependentDtoBuilder()
            .WithFamilyId(familyId)
            .WithDependentName("Child")
            .WithDependentSurname("Doe")
            .Build();
        _dependentService.IsDependentNameDuplicateAsync(familyId, "Child", "Doe", null, Arg.Any<CancellationToken>()).Returns(false);
        _dependentService.CreateDependentAsync(familyId, "Child", "Doe", Arg.Any<DateTimeOffset>(), Arg.Any<CancellationToken>()).Returns(created);

        var command = new CreateDependentCommand(familyId, "Child", "Doe", DateTimeOffset.UtcNow);
        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().BeSameAs(created);
        result.DependentName.Should().Be("Child");
    }

    [Fact]
    public async Task Handle_DuplicateDependentName_ThrowsDuplicateDependentNameException()
    {
        var familyId = Guid.NewGuid();
        _dependentService.IsDependentNameDuplicateAsync(familyId, "Duplicate", "Name", null, Arg.Any<CancellationToken>()).Returns(true);

        var command = new CreateDependentCommand(familyId, "Duplicate", "Name", DateTimeOffset.UtcNow);
        var act = () => _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<DuplicateDependentNameException>();
    }

    [Fact]
    public async Task Handle_CallsServiceWithCorrectParameters()
    {
        var familyId = Guid.NewGuid();
        var dateOfBirth = DateTimeOffset.UtcNow.AddYears(-2);
        var created = new DependentDtoBuilder().Build();
        _dependentService.IsDependentNameDuplicateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>()).Returns(false);
        _dependentService.CreateDependentAsync(familyId, "Test", "Child", dateOfBirth, Arg.Any<CancellationToken>()).Returns(created);

        var command = new CreateDependentCommand(familyId, "Test", "Child", dateOfBirth);
        await _sut.Handle(command, CancellationToken.None);

        await _dependentService.Received().CreateDependentAsync(familyId, "Test", "Child", dateOfBirth, Arg.Any<CancellationToken>());
    }
}
