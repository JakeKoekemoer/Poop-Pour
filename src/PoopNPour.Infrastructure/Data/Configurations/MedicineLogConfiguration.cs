using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoopNPour.Domain.Entities;

namespace PoopNPour.Infrastructure.Data.Configurations;

public class MedicineLogConfiguration : IEntityTypeConfiguration<MedicineLog>
{
    public void Configure(EntityTypeBuilder<MedicineLog> builder)
    {
        // Table name
        builder.ToTable("MedicineLogs");

        // PK
        builder.HasKey(ml => ml.MedicineLogId);
        builder.Property(ml => ml.MedicineLogId)
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(ml => ml.MedicineName)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(ml => ml.Dosage)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(ml => ml.TimeAdministered)
            .IsRequired();
        builder.Property(ml => ml.Notes)
            .HasConversion(
                v => string.Join("||", v),
                v => v.Split("||", StringSplitOptions.RemoveEmptyEntries).ToList()
            )
            .Metadata.SetValueComparer(
                new ValueComparer<ICollection<string>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()
                )
            );

        // Navigation
        builder.HasOne(ml => ml.Dependent)
            .WithMany(d => d.MedicineLogs)
            .HasForeignKey(ml => ml.DependentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
