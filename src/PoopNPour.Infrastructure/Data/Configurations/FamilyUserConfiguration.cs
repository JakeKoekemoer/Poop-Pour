using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoopNPour.Domain.Entities;

namespace PoopNPour.Infrastructure.Data.Configurations;

public class FamilyUserConfiguration : IEntityTypeConfiguration<FamilyUser>
{
    public void Configure(EntityTypeBuilder<FamilyUser> builder)
    {
        // Table
        builder.ToTable("FamilyUsers");

        // Composite Primary Key
        builder.HasKey(fu => new { fu.FamilyId, fu.UserId });

        // Properties
        builder.Property(fu => fu.Role)
            .IsRequired()
            .HasDefaultValue(Domain.Common.Auth.FamilyRole.Member);

        // Relationships
        builder.HasOne(fu => fu.Family)
            .WithMany(f => f.Users)
            .HasForeignKey(fu => fu.FamilyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(fu => fu.User)
            .WithMany(u => u.FamilyUsers)
            .HasForeignKey(fu => fu.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
