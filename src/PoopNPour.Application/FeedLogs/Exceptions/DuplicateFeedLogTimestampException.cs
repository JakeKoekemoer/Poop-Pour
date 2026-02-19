using PoopNPour.Application.Common.Exceptions;

namespace PoopNPour.Application.FeedLogs.Exceptions;

/// <summary>
/// Exception thrown when a feed log with the same timestamp already exists for a dependent
/// </summary>
public class DuplicateFeedLogTimestampException : DuplicateException
{
    public DuplicateFeedLogTimestampException(Guid dependentId, DateTimeOffset timeFed) 
        : base($"A feed log with timestamp '{timeFed}' already exists for dependent '{dependentId}'.")
    {
    }

    public DuplicateFeedLogTimestampException(string message) 
        : base(message)
    {
    }

    public DuplicateFeedLogTimestampException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
