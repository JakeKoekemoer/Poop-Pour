using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoopNPour.Domain.Entities;

namespace PoopNPour.Infrastructure.Data.Configurations;

public class SettingConfiguration : IEntityTypeConfiguration<Setting>
{
    public void Configure(EntityTypeBuilder<Setting> builder)
    {
        builder.ToTable("Settings");

        builder.HasKey(s => s.SettingId);

        builder.Property(s => s.SettingId)
            .ValueGeneratedOnAdd();

        builder.Property(s => s.Key)
            .IsRequired();

        builder.Property(s => s.Value)
            .IsRequired();
    }
}
