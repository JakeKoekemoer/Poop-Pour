using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PoopNPour.Infrastructure.Data;

/// <summary>
/// Used by EF Core design-time tools (dotnet ef migrations add/remove/etc.) to create
/// the DbContext without the full DI pipeline. No IUser is injected, so IsAdmin defaults
/// to true and the FamilyTenantFilter is a no-op — safe for model scaffolding.
/// </summary>
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Walk up from Infrastructure to find the Api project's appsettings
        var apiProjectPath = Path.GetFullPath(
            Path.Combine(Directory.GetCurrentDirectory(), "..", "PoopNPour.Api"));

        var config = new ConfigurationBuilder()
            .SetBasePath(apiProjectPath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = config.GetConnectionString("DefaultConnection")
            ?? "server=localhost;port=3306;database=PoopNPour_DesignTime;user=root;password=MyR0otP!ssw0rd;";

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseMySQL(connectionString)
            .Options;

        // currentUser intentionally omitted — IsAdmin defaults to true so filters are bypassed
        return new ApplicationDbContext(options);
    }
}
