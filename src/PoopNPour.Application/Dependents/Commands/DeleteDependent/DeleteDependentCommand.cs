using FluentValidation;
using MediatR;
using PoopNPour.Abstractions.Dependent;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Dependents.Exceptions;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.Dependents.Commands.DeleteDependent;

/// <summary>
/// Command to delete a dependent and all related data (FeedLogs, DiperLogs, MedicineLogs).
/// </summary>
[Authorize(Policy = Policies.CanManageDependents)]
public record DeleteDependentCommand(Guid DependentId) : IRequest<Unit>;

/// <summary>
/// Validator for DeleteDependentCommand
/// </summary>
public class DeleteDependentCommandValidator : AbstractValidator<DeleteDependentCommand>
{
    public DeleteDependentCommandValidator()
    {
        RuleFor(x => x.DependentId)
            .NotEmpty()
            .WithMessage("Dependent ID is required.");
    }
}

/// <summary>
/// Handler for DeleteDependentCommand
/// </summary>
public class DeleteDependentCommandHandler(IDependentService dependentService)
    : IRequestHandler<DeleteDependentCommand, Unit>
{
    public async Task<Unit> Handle(DeleteDependentCommand request, CancellationToken cancellationToken)
    {
        var deleted = await dependentService.DeleteDependentAsync(request.DependentId, cancellationToken);

        if (!deleted)
            throw new DependentNotFoundException(request.DependentId);

        return Unit.Value;
    }
}
