using PoopNPour.Application.Common.Exceptions;

namespace PoopNPour.Application.Users.Exceptions;

/// <summary>
/// Exception thrown when user is not found
/// </summary>
public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(string userId) 
        : base($"User with ID '{userId}' was not found.")
    {
    }

    public UserNotFoundException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
