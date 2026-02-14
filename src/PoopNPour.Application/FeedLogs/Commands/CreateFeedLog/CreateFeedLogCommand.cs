using FluentValidation;
using MediatR;
using PoopNPour.Abstractions.FeedLog;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.FeedLogs.Models;
using PoopNPour.Domain.Common.Auth;
using PoopNPour.Domain.Enums.FeedLog;

namespace PoopNPour.Application.FeedLogs.Commands.CreateFeedLog;

/// <summary>
/// Command to create a new feed log
/// </summary>
[Authorize(Policy = Policies.CanManageLogs)]
public record CreateFeedLogCommand(
    Guid DependentId,
    FeedLogType FeedType,
    DateTimeOffset TimeFed,
    decimal? MililitersFed = null,
    ICollection<string>? Notes = null) 
    : IRequest<FeedLogDto>;

/// <summary>
/// Validator for CreateFeedLogCommand
/// </summary>
public class CreateFeedLogCommandValidator : AbstractValidator<CreateFeedLogCommand>
{
    public CreateFeedLogCommandValidator()
    {
        RuleFor(x => x.DependentId)
            .NotEmpty()
            .WithMessage("Dependent ID is required.");

        RuleFor(x => x.FeedType)
            .IsInEnum()
            .WithMessage("Feed type must be a valid value.");

        RuleFor(x => x.TimeFed)
            .NotEmpty()
            .WithMessage("Time fed is required.");
    }
}

/// <summary>
/// Handler for CreateFeedLogCommand
/// </summary>
public class CreateFeedLogCommandHandler(IFeedLogService feedLogService) : IRequestHandler<CreateFeedLogCommand, FeedLogDto>
{
    public async Task<FeedLogDto> Handle(CreateFeedLogCommand request, CancellationToken cancellationToken)
    {
        return await feedLogService.CreateFeedLogAsync(
            request.DependentId,
            request.FeedType,
            request.TimeFed,
            request.MililitersFed,
            request.Notes,
            cancellationToken);
    }
}
