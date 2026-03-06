using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PoopNPour.Infrastructure.Data.Seeders;

namespace PoopNPour.Infrastructure.Data;

/// <summary>
/// Orchestrates database seeding by running all registered seeders in order.
/// </summary>
public class DatabaseSeeder(
    ApplicationDbContext context,
    IEnumerable<ISeeder> seeders,
    ILogger<DatabaseSeeder> logger)
{
    /// <summary>
    /// Seeds the database with initial data
    /// </summary>
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (context.Database.IsRelational())
            {
                await context.Database.MigrateAsync(cancellationToken);
            }
            else
            {
                await context.Database.EnsureCreatedAsync(cancellationToken);
            }

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(cancellationToken);
            }

            logger.LogInformation("Database seeding completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }
}
