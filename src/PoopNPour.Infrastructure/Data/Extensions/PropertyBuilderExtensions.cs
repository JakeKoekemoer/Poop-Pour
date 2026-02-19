using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PoopNPour.Infrastructure.Data.Extensions;

public static class PropertyBuilderExtensions
{
    /// <summary>
    /// Configures a string collection property to be stored as a delimited string in the database.
    /// Uses "||" as the delimiter and includes a custom value comparer for change tracking.
    /// Handles null values by storing them as null in the database.
    /// </summary>
    public static PropertyBuilder<ICollection<string>?> HasStringCollectionConversion(
        this PropertyBuilder<ICollection<string>?> propertyBuilder)
    {
        propertyBuilder
            .HasConversion(
                v => v == null || v.Count == 0 ? null : string.Join("||", v),
                v => string.IsNullOrEmpty(v) ? null : v.Split("||", StringSplitOptions.RemoveEmptyEntries).ToList()
            );

        propertyBuilder.Metadata.SetValueComparer(
            new ValueComparer<ICollection<string>?>(
                (c1, c2) => (c1 == null && c2 == null) || (c1 != null && c2 != null && c1.SequenceEqual(c2)),
                c => c == null ? 0 : c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c == null ? null : c.ToList()
            )
        );

        return propertyBuilder;
    }
}
