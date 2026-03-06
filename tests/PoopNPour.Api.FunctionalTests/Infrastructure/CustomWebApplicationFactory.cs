using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Api.FunctionalTests.Constants;
using PoopNPour.Infrastructure.Data;
using Xunit;

namespace PoopNPour.Api.FunctionalTests.Infrastructure;

/// <summary>
/// Custom WebApplicationFactory that boots the full API with:
/// - In-memory database (replaces SQL Server)
/// - Test configuration matching the appsettings shape (Jwt, DefaultAdmin, ApiClients, EF)
/// - Real DatabaseSeeder runs at startup to seed roles, admin, and API client users
/// - Long-lived API tokens are retrieved after seeding for use in tests
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    /// <summary>
    /// Long-lived API token for the Web_Api client, retrieved from Identity token store after seeding.
    /// </summary>
    public string WebApiToken { get; private set; } = string.Empty;

    /// <summary>
    /// Long-lived API token for the Mobile_Api client, retrieved from Identity token store after seeding.
    /// </summary>
    public string MobileApiToken { get; private set; } = string.Empty;

    private readonly string _dbName = $"TestDb_{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Do NOT clear config sources -- the app's appsettings.json must remain loaded
            // so that AddInfrastructure captures the same JWT settings used by token generation.
            // We only add overrides on top; last-added config wins for duplicate keys.
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                // Connection string (required by DI setup, even though in-memory DB replaces it)
                ["ConnectionStrings:DefaultConnection"] = "not-used",

                // Default Admin - override with test credentials
                ["DefaultAdmin:UserName"] = TestUsers.Admin.UserName,
                ["DefaultAdmin:Email"] = TestUsers.Admin.Email,
                ["DefaultAdmin:Password"] = TestUsers.Admin.Password,
                ["DefaultAdmin:FirstName"] = TestUsers.Admin.FirstName,
                ["DefaultAdmin:LastName"] = TestUsers.Admin.LastName,

                // API Clients array - override with test credentials
                ["ApiClients:0:UserName"] = TestUsers.WebApiClient.UserName,
                ["ApiClients:0:Email"] = TestUsers.WebApiClient.Email,
                ["ApiClients:0:Password"] = TestUsers.WebApiClient.Password,
                ["ApiClients:0:Role"] = TestUsers.WebApiClient.Role,
                ["ApiClients:0:FirstName"] = TestUsers.WebApiClient.FirstName,
                ["ApiClients:0:LastName"] = TestUsers.WebApiClient.LastName,

                ["ApiClients:1:UserName"] = TestUsers.MobileApiClient.UserName,
                ["ApiClients:1:Email"] = TestUsers.MobileApiClient.Email,
                ["ApiClients:1:Password"] = TestUsers.MobileApiClient.Password,
                ["ApiClients:1:Role"] = TestUsers.MobileApiClient.Role,
                ["ApiClients:1:FirstName"] = TestUsers.MobileApiClient.FirstName,
                ["ApiClients:1:LastName"] = TestUsers.MobileApiClient.LastName,

                // Disable EF retry for in-memory provider
                ["EntityFramework:EnableRetryOnFailure"] = "false",

                // Disable fake data seeding for tests
                ["SeedFakeData"] = "false",

                // Quiet logging for tests
                ["Logging:LogLevel:Default"] = "Warning",
            });
        });

        builder.ConfigureServices(services =>
        {
            // Remove ALL service descriptors related to ApplicationDbContext.
            // In EF Core 10, AddDbContext registers not only DbContextOptions<T> but also
            // IDbContextOptionsConfiguration<T> (internal) which accumulates provider calls.
            // If we only remove DbContextOptions<T>, both UseSqlServer and UseInMemoryDatabase
            // get applied, causing a "multiple database providers" error.
            var descriptorsToRemove = services.Where(d =>
            {
                var st = d.ServiceType;
                if (st == typeof(DbContextOptions<ApplicationDbContext>)) return true;
                if (st == typeof(DbContextOptions)) return true;
                if (st == typeof(ApplicationDbContext)) return true;
                // Catch internal IDbContextOptionsConfiguration<ApplicationDbContext> and similar
                if (st.IsGenericType && st.GenericTypeArguments.Length == 1 &&
                    st.GenericTypeArguments[0] == typeof(ApplicationDbContext))
                    return true;
                return false;
            }).ToList();

            foreach (var d in descriptorsToRemove)
                services.Remove(d);

            // Re-register with in-memory database (only InMemory provider)
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            });
        });
    }

    /// <summary>
    /// After the factory is built and the app has started (including DatabaseSeeder),
    /// retrieve the long-lived API tokens that were stored by the seeder.
    /// </summary>
    public async Task InitializeAsync()
    {
        // Force the server to start, which triggers Program.cs startup including DatabaseSeeder.SeedAsync()
        _ = Server;

        // Retrieve the stored API tokens
        using var scope = Services.CreateScope();
        var identityService = scope.ServiceProvider.GetRequiredService<IIdentityService>();

        // Get Web API client's stored long-lived token
        var webApiUser = await identityService.GetUserByUserNameAsync(TestUsers.WebApiClient.UserName);
        if (webApiUser != null)
        {
            WebApiToken = await identityService.GetAuthenticationTokenAsync(
                webApiUser, "PoopNPour", "ApiToken") ?? string.Empty;
        }

        // Get Mobile API client's stored long-lived token
        var mobileApiUser = await identityService.GetUserByUserNameAsync(TestUsers.MobileApiClient.UserName);
        if (mobileApiUser != null)
        {
            MobileApiToken = await identityService.GetAuthenticationTokenAsync(
                mobileApiUser, "PoopNPour", "ApiToken") ?? string.Empty;
        }
    }

    public new Task DisposeAsync() => base.DisposeAsync().AsTask();
}
