using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoopNPour.Domain.Entities;

namespace PoopNPour.Infrastructure.Data.Configurations;

public class FamilyConfiguration : IEntityTypeConfiguration<Family>
{
    public void Configure(EntityTypeBuilder<Family> builder)
    {
        // Table
        builder.ToTable("Families");

        // Primary Key
        builder.HasKey(f => f.FamilyId);

        builder.Property(f => f.FamilyId)
            .ValueGeneratedOnAdd();

        // Properties
        builder.Property(f => f.FamilyName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.FamilyLastName)
            .IsRequired()
            .HasMaxLength(100);

        // Note: Navigation property 'Users' is configured via FamilyUserConfiguration
        // This creates a many-to-many relationship through the FamilyUser junction table
    }
}
