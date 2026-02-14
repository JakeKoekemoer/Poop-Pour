namespace PoopNPour.Application.FeedLogs.Exceptions;

/// <summary>
/// Exception thrown when feed log is not found
/// </summary>
public class FeedLogNotFoundException : Exception
{
    public FeedLogNotFoundException(Guid feedLogId) 
        : base($"Feed log with ID '{feedLogId}' was not found.")
    {
    }

    public FeedLogNotFoundException(string message) 
        : base(message)
    {
    }

    public FeedLogNotFoundException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
