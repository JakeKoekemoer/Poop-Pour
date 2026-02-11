using PoopNPour.Domain.Common.Auth;

namespace PoopNPour.Api.FunctionalTests.Constants;

/// <summary>
/// Known test user credentials matching the config fed to CustomWebApplicationFactory.
/// These mirror the production appsettings shape (DefaultAdmin, ApiClients).
/// </summary>
public static class TestUsers
{
    /// <summary>
    /// Admin user seeded via the DefaultAdmin config section.
    /// Has the Administrator role assigned by DatabaseSeeder.SeedDefaultAdminAsync.
    /// </summary>
    public static class Admin
    {
        public const string UserName = "test_admin";
        public const string Email = "admin@test.poopnpour.co.za";
        public const string Password = "Admin@Test123";
        public const string FirstName = "Test";
        public const string LastName = "Admin";
    }

    /// <summary>
    /// Web API client user seeded via the ApiClients[0] config section.
    /// Has the Web_Api role and a stored long-lived API token.
    /// </summary>
    public static class WebApiClient
    {
        public const string UserName = "test_webapi";
        public const string Email = "webapi@test.poopnpour.co.za";
        public const string Password = "WebApi@Test123!Secure";
        public const string Role = Roles.Web_Api;
        public const string FirstName = "Web";
        public const string LastName = "API Client";
    }

    /// <summary>
    /// Mobile API client user seeded via the ApiClients[1] config section.
    /// Has the Mobile_Api role and a stored long-lived API token.
    /// </summary>
    public static class MobileApiClient
    {
        public const string UserName = "test_mobileapi";
        public const string Email = "mobileapi@test.poopnpour.co.za";
        public const string Password = "MobileApi@Test123!Secure";
        public const string Role = Roles.Mobile_Api;
        public const string FirstName = "Mobile";
        public const string LastName = "API Client";
    }
}
