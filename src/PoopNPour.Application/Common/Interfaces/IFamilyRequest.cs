namespace PoopNPour.Application.Common.Interfaces;

/// <summary>
/// Marker interface for MediatR requests that operate in the context of a specific family.
/// Required for FamilyAuthorizationBehavior to extract the FamilyId from the request.
/// </summary>
public interface IFamilyRequest
{
    Guid FamilyId { get; }
}
