namespace PoopNPour.Application.Common.Exceptions;

/// <summary>
/// Base exception for all "not found" scenarios
/// </summary>
public abstract class NotFoundException : Exception
{
    protected NotFoundException(string message) 
        : base(message)
    {
    }

    protected NotFoundException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
