using FluentValidation;
using MediatR;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.FamilyUsers.Exceptions;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.FamilyUsers.Commands.RemoveUserFromFamily;

/// <summary>
/// Command to remove a user from a family
/// </summary>
[Authorize(Policy = Policies.CanManageFamilies)]
public record RemoveUserFromFamilyCommand(
    Guid FamilyId,
    string UserId) 
    : IRequest<Unit>;

/// <summary>
/// Validator for RemoveUserFromFamilyCommand
/// </summary>
public class RemoveUserFromFamilyCommandValidator : AbstractValidator<RemoveUserFromFamilyCommand>
{
    public RemoveUserFromFamilyCommandValidator()
    {
        RuleFor(x => x.FamilyId)
            .NotEmpty()
            .WithMessage("Family ID is required.");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");
    }
}

/// <summary>
/// Handler for RemoveUserFromFamilyCommand
/// </summary>
public class RemoveUserFromFamilyCommandHandler(IFamilyUserService familyUserService) : IRequestHandler<RemoveUserFromFamilyCommand, Unit>
{
    public async Task<Unit> Handle(RemoveUserFromFamilyCommand request, CancellationToken cancellationToken)
    {
        var result = await familyUserService.RemoveUserFromFamilyAsync(
            request.FamilyId,
            request.UserId,
            cancellationToken);

        if (!result)
        {
            throw new FamilyUserNotFoundException(request.FamilyId, request.UserId);
        }

        return Unit.Value;
    }
}
