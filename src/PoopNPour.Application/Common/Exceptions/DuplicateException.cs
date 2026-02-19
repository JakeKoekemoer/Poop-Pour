namespace PoopNPour.Application.Common.Exceptions;

/// <summary>
/// Base exception for all "duplicate" or "already exists" scenarios
/// </summary>
public abstract class DuplicateException : Exception
{
    protected DuplicateException(string message) 
        : base(message)
    {
    }

    protected DuplicateException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
