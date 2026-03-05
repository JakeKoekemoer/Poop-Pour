using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Domain.Common.Identity;
using PoopNPour.Domain.Entities;

namespace PoopNPour.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly IUser? _currentUser;
    private string? _overrideUserId;

    private string? CurrentUserId => _overrideUserId ?? _currentUser?.Id;

    // Defaults to true when no user is present (e.g. design-time / migrations)
    private bool IsAdmin => _currentUser?.IsAdmin ?? true;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IUser? currentUser = null)
        : base(options)
    {
        _currentUser = currentUser;
    }

    /// <summary>
    /// Temporarily overrides the user ID used by the global tenant query filter.
    /// Use this when querying on behalf of a specific user whose identity is known
    /// but not yet present in the HTTP context (e.g. during login).
    /// The override is cleared when the returned IDisposable is disposed.
    /// </summary>
    public IDisposable AsUser(string userId)
    {
        _overrideUserId = userId;
        return new UserOverrideScope(() => _overrideUserId = null);
    }

    private sealed class UserOverrideScope(Action onDispose) : IDisposable
    {
        public void Dispose() => onDispose();
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

        ApplyFamilyQueryFilters(modelBuilder);
    }

    private void ApplyFamilyQueryFilters(ModelBuilder modelBuilder)
    {
        const string filterName = "FamilyTenantFilter";

        modelBuilder.Entity<Family>()
            .HasQueryFilter(filterName, f =>
                IsAdmin || f.Users.Any(fu => fu.UserId == CurrentUserId));

        modelBuilder.Entity<FamilyUser>()
            .HasQueryFilter(filterName, fu =>
                IsAdmin || fu.UserId == CurrentUserId);

        modelBuilder.Entity<Dependent>()
            .HasQueryFilter(filterName, d =>
                IsAdmin || d.Family.Users.Any(fu => fu.UserId == CurrentUserId));

        modelBuilder.Entity<DiperLog>()
            .HasQueryFilter(filterName, dl =>
                IsAdmin || dl.Dependent.Family.Users.Any(fu => fu.UserId == CurrentUserId));

        modelBuilder.Entity<FeedLog>()
            .HasQueryFilter(filterName, fl =>
                IsAdmin || fl.Dependent!.Family.Users.Any(fu => fu.UserId == CurrentUserId));

        modelBuilder.Entity<MedicineLog>()
            .HasQueryFilter(filterName, ml =>
                IsAdmin || ml.Dependent.Family.Users.Any(fu => fu.UserId == CurrentUserId));
    }
}
