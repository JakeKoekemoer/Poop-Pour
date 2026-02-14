namespace PoopNPour.Application.Dependents.Exceptions;

/// <summary>
/// Exception thrown when dependent is not found
/// </summary>
public class DependentNotFoundException : Exception
{
    public DependentNotFoundException(Guid dependentId) 
        : base($"Dependent with ID '{dependentId}' was not found.")
    {
    }

    public DependentNotFoundException(string message) 
        : base(message)
    {
    }

    public DependentNotFoundException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
