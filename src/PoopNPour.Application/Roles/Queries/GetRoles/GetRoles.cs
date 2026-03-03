using System.Collections.Generic;
using System.Reflection;
using MediatR;
using PoopNPour.Application.Authorization;
using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Application.Roles.Queries;

[Authorize(Policy = Policies.CanViewRoles)]
public record GetRolesQuery() : IRequest<IReadOnlyList<string>>;

public class GetRolesQueryHandler()
    : IRequestHandler<GetRolesQuery, IReadOnlyList<string>>
{
    public Task<IReadOnlyList<string>> Handle(
        GetRolesQuery request,
        CancellationToken cancellationToken)
    {
        var fields = typeof(Domain.Common.Auth.Roles)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(f => f.FieldType == typeof(string));

        var roles = fields
            .Select(f => (string)f.GetValue(null)!)
            .OrderBy(name => name)
            .ToArray();

        return Task.FromResult<IReadOnlyList<string>>(roles);
    }
}

