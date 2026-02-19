using PoopNPour.Application.Common.Exceptions;

namespace PoopNPour.Application.Families.Exceptions;

/// <summary>
/// Exception thrown when a family name already exists for a user
/// </summary>
public class DuplicateFamilyNameException : DuplicateException
{
    public DuplicateFamilyNameException(string familyName, string userId) 
        : base($"A family with the name '{familyName}' already exists for user '{userId}'.")
    {
    }

    public DuplicateFamilyNameException(string message) 
        : base(message)
    {
    }

    public DuplicateFamilyNameException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
