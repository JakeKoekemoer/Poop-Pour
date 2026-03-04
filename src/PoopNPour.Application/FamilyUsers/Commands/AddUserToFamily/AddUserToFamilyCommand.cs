using FluentValidation;
using MediatR;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Common.Interfaces;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.FamilyUsers.Commands.AddUserToFamily;

/// <summary>
/// Command to add a user to a family
/// </summary>
[Authorize(Policy = Policies.CanManageFamilies)]
[AuthorizeFamilyMember(FamilyRole.Owner)]
public record AddUserToFamilyCommand(
    Guid FamilyId,
    string UserId) 
    : IRequest<FamilyUserDto>, IFamilyRequest;

/// <summary>
/// Validator for AddUserToFamilyCommand
/// </summary>
public class AddUserToFamilyCommandValidator : AbstractValidator<AddUserToFamilyCommand>
{
    public AddUserToFamilyCommandValidator()
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
/// Handler for AddUserToFamilyCommand
/// </summary>
public class AddUserToFamilyCommandHandler(IFamilyUserService familyUserService) : IRequestHandler<AddUserToFamilyCommand, FamilyUserDto>
{
    public async Task<FamilyUserDto> Handle(AddUserToFamilyCommand request, CancellationToken cancellationToken)
    {
        return await familyUserService.AddUserToFamilyAsync(
            request.FamilyId,
            request.UserId,
            cancellationToken);
    }
}
