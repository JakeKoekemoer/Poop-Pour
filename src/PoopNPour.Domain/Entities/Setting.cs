using PoopNPour.Domain.Common;

namespace PoopNPour.Domain.Entities;

public class Setting : BaseAuditableEntity
{
    public Setting()
    {
        CreatedOn = DateTimeOffset.UtcNow;
        LastModifiedOn = DateTimeOffset.UtcNow;
    }

    public int SettingId { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

[AttributeUsage(AttributeTargets.Property)]
public class SettingComplexType : Attribute { }
