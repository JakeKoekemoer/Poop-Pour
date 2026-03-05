using FluentValidation;
using MediatR;
using PoopNPour.Abstractions.Family;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Common.Interfaces;
using PoopNPour.Application.Families.Exceptions;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.Families.Commands.DeleteFamily;

/// <summary>
/// Command to delete a family and all related data (FamilyUsers, Dependents, logs).
/// </summary>
[Authorize(Policy = Policies.CanManageFamilies)]
[AuthorizeFamilyMember(FamilyRole.Owner)]
public record DeleteFamilyCommand(Guid FamilyId)
    : IRequest<Unit>, IFamilyRequest;

/// <summary>
/// Validator for DeleteFamilyCommand
/// </summary>
public class DeleteFamilyCommandValidator : AbstractValidator<DeleteFamilyCommand>
{
    public DeleteFamilyCommandValidator()
    {
        RuleFor(x => x.FamilyId)
            .NotEmpty()
            .WithMessage("Family ID is required.");
    }
}

/// <summary>
/// Handler for DeleteFamilyCommand
/// </summary>
public class DeleteFamilyCommandHandler(IFamilyService familyService)
    : IRequestHandler<DeleteFamilyCommand, Unit>
{
    public async Task<Unit> Handle(DeleteFamilyCommand request, CancellationToken cancellationToken)
    {
        var deleted = await familyService.DeleteFamilyAsync(request.FamilyId, cancellationToken);

        if (!deleted)
            throw new FamilyNotFoundException(request.FamilyId);

        return Unit.Value;
    }
}
