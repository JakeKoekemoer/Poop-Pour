using PoopNPour.Application.Common.Exceptions;

namespace PoopNPour.Application.DiperLogs.Exceptions;

/// <summary>
/// Exception thrown when diper log is not found
/// </summary>
public class DiperLogNotFoundException : NotFoundException
{
    public DiperLogNotFoundException(Guid diperLogId) 
        : base($"Diper log with ID '{diperLogId}' was not found.")
    {
    }

    public DiperLogNotFoundException(string message) 
        : base(message)
    {
    }

    public DiperLogNotFoundException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
