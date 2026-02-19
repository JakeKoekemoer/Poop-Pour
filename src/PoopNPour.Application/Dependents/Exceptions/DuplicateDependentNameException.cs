using PoopNPour.Application.Common.Exceptions;

namespace PoopNPour.Application.Dependents.Exceptions;

/// <summary>
/// Exception thrown when a dependent name already exists in a family
/// </summary>
public class DuplicateDependentNameException : DuplicateException
{
    public DuplicateDependentNameException(string dependentName, string dependentSurname, Guid familyId) 
        : base($"A dependent with the name '{dependentName} {dependentSurname}' already exists in family '{familyId}'.")
    {
    }

    public DuplicateDependentNameException(string message) 
        : base(message)
    {
    }

    public DuplicateDependentNameException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
