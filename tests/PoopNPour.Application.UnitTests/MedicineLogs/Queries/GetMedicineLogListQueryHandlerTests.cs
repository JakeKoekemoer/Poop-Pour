using FluentAssertions;
using NSubstitute;
using PoopNPour.Abstractions.MedicineLog;
using PoopNPour.Application.MedicineLogs.Queries.GetMedicineLogList;
using PoopNPour.Application.UnitTests.TestHelpers;
using Xunit;

namespace PoopNPour.Application.UnitTests.MedicineLogs.Queries;

public class GetMedicineLogListQueryHandlerTests
{
    private readonly IMedicineLogService _medicineLogService;
    private readonly GetMedicineLogListQueryHandler _sut;

    public GetMedicineLogListQueryHandlerTests()
    {
        _medicineLogService = Substitute.For<IMedicineLogService>();
        _sut = new GetMedicineLogListQueryHandler(_medicineLogService);
    }

    [Fact]
    public async Task Handle_ReturnsPaginatedResults()
    {
        var logs = new[] { new MedicineLogDtoBuilder().Build(), new MedicineLogDtoBuilder().Build() };
        _medicineLogService.GetMedicineLogsAsync(1, 10, null, null, null, null, Arg.Any<CancellationToken>()).Returns((logs, 2));

        var query = new GetMedicineLogListQuery(1, 10);
        var result = await _sut.Handle(query, CancellationToken.None);

        result.Items.Should().HaveCount(2);
        result.PageNumber.Should().Be(1);
    }
}
