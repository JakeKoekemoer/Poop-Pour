namespace PoopNPour.Application.DiperLogs.Exceptions;

/// <summary>
/// Exception thrown when a diaper log with the same timestamp already exists for a dependent
/// </summary>
public class DuplicateDiperLogTimestampException : Exception
{
    public DuplicateDiperLogTimestampException(Guid dependentId, DateTimeOffset diperDate) 
        : base($"A diaper log with timestamp '{diperDate}' already exists for dependent '{dependentId}'.")
    {
    }

    public DuplicateDiperLogTimestampException(string message) 
        : base(message)
    {
    }

    public DuplicateDiperLogTimestampException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
