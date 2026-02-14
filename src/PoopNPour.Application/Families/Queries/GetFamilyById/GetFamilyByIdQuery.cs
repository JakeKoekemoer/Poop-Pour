using MediatR;
using PoopNPour.Abstractions.Family;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Families.Exceptions;
using PoopNPour.Application.Families.Models;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.Families.Queries.GetFamilyById;

/// <summary>
/// Query to get a family by ID
/// </summary>
[Authorize(Policy = Policies.CanViewFamilies)]
public record GetFamilyByIdQuery(Guid FamilyId) 
    : IRequest<FamilyDto>;

/// <summary>
/// Handler for GetFamilyByIdQuery
/// </summary>
public class GetFamilyByIdQueryHandler(IFamilyService familyService) : IRequestHandler<GetFamilyByIdQuery, FamilyDto>
{
    public async Task<FamilyDto> Handle(GetFamilyByIdQuery request, CancellationToken cancellationToken)
    {
        var family = await familyService.GetFamilyByIdAsync(request.FamilyId, cancellationToken);

        if (family == null)
        {
            throw new FamilyNotFoundException(request.FamilyId);
        }

        return family;
    }
}
