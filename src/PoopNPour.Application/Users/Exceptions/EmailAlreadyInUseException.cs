using PoopNPour.Application.Common.Exceptions;

namespace PoopNPour.Application.Users.Exceptions;

/// <summary>
/// Exception thrown when updating profile with existing email
/// </summary>
public class EmailAlreadyInUseException : DuplicateException
{
    public EmailAlreadyInUseException(string email) 
        : base($"Email '{email}' is already in use by another user.")
    {
    }

    public EmailAlreadyInUseException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
