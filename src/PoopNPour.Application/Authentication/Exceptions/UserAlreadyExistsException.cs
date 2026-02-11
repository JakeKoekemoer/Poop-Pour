namespace PoopNPour.Application.Authentication.Exceptions;

/// <summary>
/// Exception thrown when registering with existing email or username
/// </summary>
public class UserAlreadyExistsException : Exception
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
