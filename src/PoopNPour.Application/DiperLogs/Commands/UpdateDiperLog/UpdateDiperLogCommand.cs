using FluentValidation;
using MediatR;
using PoopNPour.Abstractions.DiperLog;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.DiperLogs.Exceptions;
using PoopNPour.Domain.Common.Auth;
using PoopNPour.Domain.Enums.DiperLog;

namespace PoopNPour.Application.DiperLogs.Commands.UpdateDiperLog;

/// <summary>
/// Command to update an existing diper log
/// </summary>
[Authorize(Policy = Policies.CanManageLogs)]
public record UpdateDiperLogCommand(
    Guid DiperLogId,
    DateTimeOffset? DiperDate = null,
    FecalDischargeColour? FecalDischargeColour = null,
    UrinalDischargeColour? UrinaryDischargeColour = null,
    ICollection<string>? Notes = null) 
    : IRequest<DiperLogDto>;

/// <summary>
/// Validator for UpdateDiperLogCommand
/// </summary>
public class UpdateDiperLogCommandValidator : AbstractValidator<UpdateDiperLogCommand>
{
    public UpdateDiperLogCommandValidator()
    {
        RuleFor(x => x.DiperLogId)
            .NotEmpty()
            .WithMessage("Diper log ID is required.");

        When(x => x.FecalDischargeColour.HasValue, () =>
        {
            RuleFor(x => x.FecalDischargeColour)
                .IsInEnum()
                .WithMessage("Fecal discharge colour must be a valid value.");
        });

        When(x => x.UrinaryDischargeColour.HasValue, () =>
        {
            RuleFor(x => x.UrinaryDischargeColour)
                .IsInEnum()
                .WithMessage("Urinary discharge colour must be a valid value.");
        });
    }
}

/// <summary>
/// Handler for UpdateDiperLogCommand
/// </summary>
public class UpdateDiperLogCommandHandler(IDiperLogService diperLogService) : IRequestHandler<UpdateDiperLogCommand, DiperLogDto>
{
    public async Task<DiperLogDto> Handle(UpdateDiperLogCommand request, CancellationToken cancellationToken)
    {
        var diperLog = await diperLogService.UpdateDiperLogAsync(
            request.DiperLogId,
            request.DiperDate,
            request.FecalDischargeColour,
            request.UrinaryDischargeColour,
            request.Notes,
            cancellationToken);

        if (diperLog == null)
        {
            throw new DiperLogNotFoundException(request.DiperLogId);
        }

        return diperLog;
    }
}
