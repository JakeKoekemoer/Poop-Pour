using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PoopNPour.Domain.Common.Identity;
using PoopNPour.Domain.Entities;

namespace PoopNPour.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Setting> Settings { get; set; } = null!;
    public DbSet<Family> Families { get; set; } = null!;
    public DbSet<FamilyUser> FamilyUsers { get; set; } = null!;
    public DbSet<Dependent> Dependents { get; set; } = null!;
    public DbSet<FeedLog> FeedLogs { get; set; } = null!;
    public DbSet<DiperLog> DiperLogs { get; set; } = null!;
    public DbSet<MedicineLog> MedicineLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
