using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.MedicineLog;
using PoopNPour.Application.MedicineLogs.Commands.CreateMedicineLog;
using PoopNPour.Application.MedicineLogs.Exceptions;
using PoopNPour.Application.UnitTests.TestHelpers;
using Xunit;

namespace PoopNPour.Application.UnitTests.MedicineLogs.Commands;

public class CreateMedicineLogCommandHandlerTests
{
    private readonly IMedicineLogService _medicineLogService;
    private readonly CreateMedicineLogCommandHandler _sut;

    public CreateMedicineLogCommandHandlerTests()
    {
        _medicineLogService = Substitute.For<IMedicineLogService>();
        _sut = new CreateMedicineLogCommandHandler(_medicineLogService);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesMedicineLogSuccessfully()
    {
        var dependentId = Guid.NewGuid();
        var created = new MedicineLogDtoBuilder().WithDependentId(dependentId).Build();
        _medicineLogService.IsMedicineLogDuplicateAsync(dependentId, "Paracetamol", Arg.Any<DateTimeOffset>(), null, Arg.Any<CancellationToken>()).Returns(false);
        _medicineLogService.CreateMedicineLogAsync(dependentId, "Paracetamol", "5ml", Arg.Any<DateTimeOffset>(), null, Arg.Any<CancellationToken>()).Returns(created);

        var command = new CreateMedicineLogCommand(dependentId, "Paracetamol", "5ml", DateTimeOffset.UtcNow, null);
        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().BeSameAs(created);
    }

    [Fact]
    public async Task Handle_DuplicateMedicineAndTimestamp_ThrowsDuplicateMedicineLogException()
    {
        var dependentId = Guid.NewGuid();
        var timestamp = DateTimeOffset.UtcNow;
        _medicineLogService.IsMedicineLogDuplicateAsync(dependentId, "Paracetamol", timestamp, null, Arg.Any<CancellationToken>()).Returns(true);

        var command = new CreateMedicineLogCommand(dependentId, "Paracetamol", "5ml", timestamp);
        var act = () => _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<DuplicateMedicineLogException>();
    }
}
