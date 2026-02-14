using MediatR;
using PoopNPour.Abstractions.Dependent;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Dependents.Exceptions;
using PoopNPour.Application.Dependents.Models;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.Dependents.Queries.GetDependentById;

/// <summary>
/// Query to get a dependent by ID
/// </summary>
[Authorize(Policy = Policies.CanViewDependents)]
public record GetDependentByIdQuery(Guid DependentId) 
    : IRequest<DependentDto>;

/// <summary>
/// Handler for GetDependentByIdQuery
/// </summary>
public class GetDependentByIdQueryHandler(IDependentService dependentService) : IRequestHandler<GetDependentByIdQuery, DependentDto>
{
    public async Task<DependentDto> Handle(GetDependentByIdQuery request, CancellationToken cancellationToken)
    {
        var dependent = await dependentService.GetDependentByIdAsync(request.DependentId, cancellationToken);

        if (dependent == null)
        {
            throw new DependentNotFoundException(request.DependentId);
        }

        return dependent;
    }
}
