using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoopNPour.Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace PoopNPour.Infrastructure.Data.Configurations;

public class DiperLogConfiguration : IEntityTypeConfiguration<DiperLog>
{
    public void Configure(EntityTypeBuilder<DiperLog> builder)
    {
        // Table name
        builder.ToTable("DiperLogs");

        // PK
        builder.HasKey(d => d.DiperLogId);
        builder.Property(d => d.DiperLogId)
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(d => d.DependentId)
            .IsRequired();
        builder.Property(d => d.DiperDate)
            .IsRequired();
        builder.Property(d => d.FecalDischargeColour)
            .IsRequired();
        builder.Property(d => d.UrinaryDischargeColour)
            .IsRequired();
        builder.Property(d => d.Notes)
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
        builder.HasOne(d => d.Dependent)
            .WithMany(dep => dep.DiperLogs)
            .HasForeignKey(d => d.DependentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
