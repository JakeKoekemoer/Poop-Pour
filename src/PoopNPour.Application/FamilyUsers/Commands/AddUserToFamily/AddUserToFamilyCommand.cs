using FluentValidation;
using MediatR;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Abstractions.User;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Common.Interfaces;
using PoopNPour.Application.Users.Exceptions;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.FamilyUsers.Commands.AddUserToFamily;

/// <summary>
/// Command to add a user to a family by their email address.
/// The handler resolves the email to a user ID via IUserService before adding.
/// </summary>
[Authorize(Policy = Policies.CanManageFamilies)]
[AuthorizeFamilyMember(FamilyRole.Owner)]
public record AddUserToFamilyCommand(
    Guid FamilyId,
    string Email)
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

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("A valid email address is required.");
    }
}

/// <summary>
/// Handler for AddUserToFamilyCommand
/// </summary>
public class AddUserToFamilyCommandHandler(
    IFamilyUserService familyUserService,
    IUserService userService)
    : IRequestHandler<AddUserToFamilyCommand, FamilyUserDto>
{
    public async Task<FamilyUserDto> Handle(AddUserToFamilyCommand request, CancellationToken cancellationToken)
    {
        var user = await userService.GetUserByEmailAsync(request.Email, cancellationToken);
        if (user is null)
            throw new UserNotFoundException(request.Email);

        return await familyUserService.AddUserToFamilyAsync(
            request.FamilyId,
            user.Id,
            cancellationToken);
    }
}
