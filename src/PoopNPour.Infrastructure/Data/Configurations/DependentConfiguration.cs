using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoopNPour.Domain.Entities;

namespace PoopNPour.Infrastructure.Data.Configurations;

public class DependentConfiguration : IEntityTypeConfiguration<Dependent>
{
    public void Configure(EntityTypeBuilder<Dependent> builder)
    {
        // Table name
        builder.ToTable("Dependents");

        // Primary key
        builder.HasKey(d => d.DependentId);
        builder.Property(d => d.DependentId)
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(d => d.DependentName)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(d => d.DependentSurname)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(d => d.DateOfBirth)
            .IsRequired();

        // Ignore Age as it's calculated
        builder.Ignore(d => d.Age);

        // Navigation properties
        builder.HasOne(d => d.Family)
            .WithMany(f => f.Dependents)
            .HasForeignKey(d => d.FamilyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.FeedLogs)
            .WithOne(fl => fl.Dependent)
            .HasForeignKey(fl => fl.DependentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
