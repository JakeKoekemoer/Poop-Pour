namespace PoopNPour.Application.FamilyUsers.Exceptions;

/// <summary>
/// Exception thrown when a user is already in a family
/// </summary>
public class UserAlreadyInFamilyException : Exception
{
    public UserAlreadyInFamilyException(Guid familyId, string userId) 
        : base($"User '{userId}' is already a member of family '{familyId}'.")
    {
    }

    public UserAlreadyInFamilyException(string message) 
        : base(message)
    {
    }

    public UserAlreadyInFamilyException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
