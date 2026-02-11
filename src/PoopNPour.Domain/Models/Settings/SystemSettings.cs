using PoopNPour.Domain.Interfaces.Settings;

namespace PoopNPour.Domain.Models.Settings;

public class SystemSettings : ISettings
{
    public bool SetupCompleted { get; set; }
}
