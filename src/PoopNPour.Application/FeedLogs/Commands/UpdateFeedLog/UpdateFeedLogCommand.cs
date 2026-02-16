using FluentValidation;
using MediatR;
using PoopNPour.Abstractions.FeedLog;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.FeedLogs.Exceptions;
using PoopNPour.Domain.Common.Auth;
using PoopNPour.Domain.Enums.FeedLog;

namespace PoopNPour.Application.FeedLogs.Commands.UpdateFeedLog;

/// <summary>
/// Command to update an existing feed log
/// </summary>
[Authorize(Policy = Policies.CanManageLogs)]
public record UpdateFeedLogCommand(
    Guid FeedLogId,
    FeedLogType? FeedType = null,
    DateTimeOffset? TimeFed = null,
    decimal? MililitersFed = null,
    ICollection<string>? Notes = null) 
    : IRequest<FeedLogDto>;

/// <summary>
/// Validator for UpdateFeedLogCommand
/// </summary>
public class UpdateFeedLogCommandValidator : AbstractValidator<UpdateFeedLogCommand>
{
    public UpdateFeedLogCommandValidator()
    {
        RuleFor(x => x.FeedLogId)
            .NotEmpty()
            .WithMessage("Feed log ID is required.");

        When(x => x.FeedType.HasValue, () =>
        {
            RuleFor(x => x.FeedType)
                .IsInEnum()
                .WithMessage("Feed type must be a valid value.");
        });
    }
}

/// <summary>
/// Handler for UpdateFeedLogCommand
/// </summary>
public class UpdateFeedLogCommandHandler(IFeedLogService feedLogService) : IRequestHandler<UpdateFeedLogCommand, FeedLogDto>
{
    public async Task<FeedLogDto> Handle(UpdateFeedLogCommand request, CancellationToken cancellationToken)
    {
        var feedLog = await feedLogService.UpdateFeedLogAsync(
            request.FeedLogId,
            request.FeedType,
            request.TimeFed,
            request.MililitersFed,
            request.Notes,
            cancellationToken);

        if (feedLog == null)
        {
            throw new FeedLogNotFoundException(request.FeedLogId);
        }

        return feedLog;
    }
}
