namespace PoopNPour.Domain.Entities;

public class Setting
{
    public Setting()
    {
        CreatedOnUtc = DateTime.UtcNow;
        ModifiedOnUtc = DateTime.UtcNow;
    }

    public int SettingId { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime ModifiedOnUtc { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

[AttributeUsage(AttributeTargets.Property)]
public class SettingComplexType : Attribute { }
