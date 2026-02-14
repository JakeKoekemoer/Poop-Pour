namespace PoopNPour.Application.Families.Exceptions;

/// <summary>
/// Exception thrown when family is not found
/// </summary>
public class FamilyNotFoundException : Exception
{
    public FamilyNotFoundException(Guid familyId) 
        : base($"Family with ID '{familyId}' was not found.")
    {
    }

    public FamilyNotFoundException(string message) 
        : base(message)
    {
    }

    public FamilyNotFoundException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
