using FluentValidation;
using MediatR;
using PoopNPour.Abstractions.DiperLog;
using PoopNPour.Application.Authorization;
using PoopNPour.Domain.Common.Auth;
using PoopNPour.Domain.Enums.DiperLog;

namespace PoopNPour.Application.DiperLogs.Commands.CreateDiperLog;

/// <summary>
/// Command to create a new diper log
/// </summary>
[Authorize(Policy = Policies.CanManageLogs)]
public record CreateDiperLogCommand(
    Guid DependentId,
    DateTimeOffset DiperDate,
    FecalDischargeColour FecalDischargeColour,
    UrinalDischargeColour UrinaryDischargeColour,
    ICollection<string>? Notes = null) 
    : IRequest<DiperLogDto>;

/// <summary>
/// Validator for CreateDiperLogCommand
/// </summary>
public class CreateDiperLogCommandValidator : AbstractValidator<CreateDiperLogCommand>
{
    public CreateDiperLogCommandValidator()
    {
        RuleFor(x => x.DependentId)
            .NotEmpty()
            .WithMessage("Dependent ID is required.");

        RuleFor(x => x.DiperDate)
            .NotEmpty()
            .WithMessage("Diper date is required.");

        RuleFor(x => x.FecalDischargeColour)
            .IsInEnum()
            .WithMessage("Fecal discharge colour must be a valid value.");

        RuleFor(x => x.UrinaryDischargeColour)
            .IsInEnum()
            .WithMessage("Urinary discharge colour must be a valid value.");
    }
}

/// <summary>
/// Handler for CreateDiperLogCommand
/// </summary>
public class CreateDiperLogCommandHandler(IDiperLogService diperLogService) : IRequestHandler<CreateDiperLogCommand, DiperLogDto>
{
    public async Task<DiperLogDto> Handle(CreateDiperLogCommand request, CancellationToken cancellationToken)
    {
        return await diperLogService.CreateDiperLogAsync(
            request.DependentId,
            request.DiperDate,
            request.FecalDischargeColour,
            request.UrinaryDischargeColour,
            request.Notes,
            cancellationToken);
    }
}
