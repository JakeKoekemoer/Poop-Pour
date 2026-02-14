namespace PoopNPour.Application.MedicineLogs.Exceptions;

/// <summary>
/// Exception thrown when medicine log is not found
/// </summary>
public class MedicineLogNotFoundException : Exception
{
    public MedicineLogNotFoundException(Guid medicineLogId) 
        : base($"Medicine log with ID '{medicineLogId}' was not found.")
    {
    }

    public MedicineLogNotFoundException(string message) 
        : base(message)
    {
    }

    public MedicineLogNotFoundException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
