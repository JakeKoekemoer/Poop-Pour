using FluentValidation.TestHelper;
using PoopNPour.Application.FeedLogs.Commands.CreateFeedLog;
using PoopNPour.Domain.Enums.FeedLog;
using Xunit;

namespace PoopNPour.Application.UnitTests.FeedLogs.Commands;

public class CreateFeedLogValidatorValidCommandPassesValidation
{
    private readonly CreateFeedLogCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_PassesValidation()
    {
        var command = new CreateFeedLogCommand(
            DependentId: Guid.NewGuid(),
            FeedType: FeedLogType.BREAST_MILK,
            TimeFed: DateTimeOffset.UtcNow,
            MililitersFed: 120m);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
