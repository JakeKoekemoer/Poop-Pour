using PoopNPour.Application.Common.Exceptions;

namespace PoopNPour.Application.Users.Exceptions;

/// <summary>
/// Exception thrown when user creation fails due to Identity errors
/// </summary>
public class UserCreationFailedException : CreateFailedException
{
    public UserCreationFailedException(string message)
        : base(message)
    {
    }

    public UserCreationFailedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
