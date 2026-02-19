using PoopNPour.Application.Common.Exceptions;

namespace PoopNPour.Application.Authentication.Exceptions;

/// <summary>
/// Exception thrown when registering with existing email or username
/// </summary>
public class UserAlreadyExistsException : DuplicateException
{
    public UserAlreadyExistsException(string emailOrUserName) 
        : base($"A user with email or username '{emailOrUserName}' already exists.")
    {
    }

    public UserAlreadyExistsException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
