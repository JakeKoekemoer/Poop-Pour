using MediatR;
using PoopNPour.Abstractions.FamilyUser;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.FamilyUsers.Exceptions;
using PoopNPour.Application.FamilyUsers.Models;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.FamilyUsers.Queries.GetFamilyUserById;

/// <summary>
/// Query to get a family user by composite key
/// </summary>
[Authorize(Policy = Policies.CanViewFamilies)]
public record GetFamilyUserByIdQuery(
    Guid FamilyId,
    string UserId) 
    : IRequest<FamilyUserDto>;

/// <summary>
/// Handler for GetFamilyUserByIdQuery
/// </summary>
public class GetFamilyUserByIdQueryHandler(IFamilyUserService familyUserService) : IRequestHandler<GetFamilyUserByIdQuery, FamilyUserDto>
{
    public async Task<FamilyUserDto> Handle(GetFamilyUserByIdQuery request, CancellationToken cancellationToken)
    {
        var familyUser = await familyUserService.GetFamilyUserByIdAsync(
            request.FamilyId, 
            request.UserId, 
            cancellationToken);

        if (familyUser == null)
        {
            throw new FamilyUserNotFoundException(request.FamilyId, request.UserId);
        }

        return familyUser;
    }
}
