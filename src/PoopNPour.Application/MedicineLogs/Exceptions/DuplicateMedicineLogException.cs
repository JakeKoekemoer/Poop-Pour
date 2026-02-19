namespace PoopNPour.Application.MedicineLogs.Exceptions;

/// <summary>
/// Exception thrown when the same medicine has already been administered at the same time for a dependent
/// </summary>
public class DuplicateMedicineLogException : Exception
{
    public DuplicateMedicineLogException(Guid dependentId, string medicineName, DateTimeOffset timeAdministered) 
        : base($"Medicine '{medicineName}' has already been administered at '{timeAdministered}' for dependent '{dependentId}'.")
    {
    }

    public DuplicateMedicineLogException(string message) 
        : base(message)
    {
    }

    public DuplicateMedicineLogException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
