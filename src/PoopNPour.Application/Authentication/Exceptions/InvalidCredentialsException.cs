namespace PoopNPour.Application.Authentication.Exceptions;

/// <summary>
/// Exception thrown when login credentials are invalid
/// </summary>
public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException() : base("Invalid email/username or password.")
    {
    }

    public InvalidCredentialsException(string message) : base(message)
    {
    }
}
