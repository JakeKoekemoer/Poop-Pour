namespace PoopNPour.Application.Common.Exceptions;

/// <summary>
/// Base exception for all "creation failed" scenarios
/// </summary>
public abstract class CreateFailedException : Exception
{
    protected CreateFailedException(string message)
        : base(message)
    {
    }

    protected CreateFailedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
