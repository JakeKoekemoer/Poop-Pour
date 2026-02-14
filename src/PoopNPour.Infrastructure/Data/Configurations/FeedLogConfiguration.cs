using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoopNPour.Domain.Entities;

namespace PoopNPour.Infrastructure.Data.Configurations;

public class FeedLogConfiguration : IEntityTypeConfiguration<FeedLog>
{
    public void Configure(EntityTypeBuilder<FeedLog> builder)
    {
        // Table name
        builder.ToTable("FeedLogs");

        // PK
        builder.HasKey(fl => fl.FeedLogId);
        builder.Property(fl => fl.FeedLogId)
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(fl => fl.DependentId)
            .IsRequired();
        builder.Property(fl => fl.FeedType)
            .IsRequired();
        builder.Property(fl => fl.TimeFed)
            .IsRequired();
        builder.Property(fl => fl.MililitersFed)
            .HasColumnType("decimal(18,2)");
        builder.Property(fl => fl.Notes)
            .HasConversion(
                v => string.Join("||", v),
                v => v.Split("||", StringSplitOptions.RemoveEmptyEntries).ToList()
            )
            .Metadata.SetValueComparer(
                new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<ICollection<string>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()
                )
            );

        // Navigation
        builder.HasOne(fl => fl.Dependent)
            .WithMany(d => d.FeedLogs)
            .HasForeignKey(fl => fl.DependentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
