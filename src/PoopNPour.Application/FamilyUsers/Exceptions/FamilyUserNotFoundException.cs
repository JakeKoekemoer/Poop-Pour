using PoopNPour.Application.Common.Exceptions;

namespace PoopNPour.Application.FamilyUsers.Exceptions;

/// <summary>
/// Exception thrown when family user is not found
/// </summary>
public class FamilyUserNotFoundException : NotFoundException
{
    public FamilyUserNotFoundException(Guid familyId, string userId) 
        : base($"User '{userId}' is not found in family '{familyId}'.")
    {
    }

    public FamilyUserNotFoundException(string message) 
        : base(message)
    {
    }

    public FamilyUserNotFoundException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
