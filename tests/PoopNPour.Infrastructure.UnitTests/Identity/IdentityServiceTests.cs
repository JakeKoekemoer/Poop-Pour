using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PoopNPour.Domain.Common.Auth;
using PoopNPour.Domain.Common.Identity;
using PoopNPour.Infrastructure.Authorization;
using PoopNPour.Infrastructure.Data;
using PoopNPour.Infrastructure.Identity;
using Xunit;

namespace PoopNPour.Infrastructure.UnitTests.Identity;

public class IdentityServiceTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IAuthorizationService _authorizationService;
    private readonly IdentityService _sut;

    public IdentityServiceTests()
    {
        var services = new ServiceCollection();
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}"));
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 1;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddAuthorization(options => PolicyRoleMappings.AddRolesToPolicies(options));
        services.AddLogging();

        var serviceProvider = services.BuildServiceProvider();
        _context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        _authorizationService = serviceProvider.GetRequiredService<IAuthorizationService>();

        _sut = new IdentityService(_userManager, _roleManager, _authorizationService);
    }

    [Fact]
    public async Task GetUserByUserNameAsync_UserExists_ReturnsUser()
    {
        var user = new ApplicationUser { UserName = "testuser", Email = "test@test.com" };
        await _userManager.CreateAsync(user);

        var result = await _sut.GetUserByUserNameAsync("testuser");

        result.Should().NotBeNull();
        result!.UserName.Should().Be("testuser");
    }

    [Fact]
    public async Task GetUserByUserNameAsync_UserDoesNotExist_ReturnsNull()
    {
        var result = await _sut.GetUserByUserNameAsync("nonexistent");

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetUserByEmailAsync_UserExists_ReturnsUser()
    {
        var user = new ApplicationUser { UserName = "testuser", Email = "test@test.com" };
        await _userManager.CreateAsync(user);

        var result = await _sut.GetUserByEmailAsync("test@test.com");

        result.Should().NotBeNull();
        result!.Email.Should().Be("test@test.com");
    }

    [Fact]
    public async Task IsInRoleAsync_UserHasRole_ReturnsTrue()
    {
        var role = new IdentityRole(Roles.Administrator);
        await _roleManager.CreateAsync(role);
        var user = new ApplicationUser { UserName = "admin", Email = "admin@test.com" };
        await _userManager.CreateAsync(user);
        await _userManager.AddToRoleAsync(user, Roles.Administrator);

        var result = await _sut.IsInRoleAsync(user.Id, Roles.Administrator);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsInRoleAsync_UserDoesNotHaveRole_ReturnsFalse()
    {
        var user = new ApplicationUser { UserName = "user", Email = "user@test.com" };
        await _userManager.CreateAsync(user);

        var result = await _sut.IsInRoleAsync(user.Id, Roles.Administrator);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task IsInRoleAsync_UserDoesNotExist_ReturnsFalse()
    {
        var result = await _sut.IsInRoleAsync("nonexistent-id", Roles.Administrator);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task AuthorizeAsync_UserSatisfiesPolicy_ReturnsTrue()
    {
        var role = new IdentityRole(Roles.Administrator);
        await _roleManager.CreateAsync(role);
        var user = new ApplicationUser { UserName = "admin", Email = "admin@test.com" };
        await _userManager.CreateAsync(user);
        await _userManager.AddToRoleAsync(user, Roles.Administrator);

        var result = await _sut.AuthorizeAsync(user.Id, Policies.CanViewUsers);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task AuthorizeAsync_UserDoesNotSatisfyPolicy_ReturnsFalse()
    {
        var role = new IdentityRole(Roles.Tenant);
        await _roleManager.CreateAsync(role);
        var user = new ApplicationUser { UserName = "member", Email = "member@test.com" };
        await _userManager.CreateAsync(user);
        await _userManager.AddToRoleAsync(user, Roles.Tenant);

        var result = await _sut.AuthorizeAsync(user.Id, Policies.CanViewUsers);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task CreateUserAsync_CreatesUserWithCorrectProperties()
    {
        var result = await _sut.CreateUserAsync(
            "newuser",
            "new@test.com",
            "password",
            "First",
            "Last");

        result.Should().NotBeNull();
        result.UserName.Should().Be("newuser");
        result.Email.Should().Be("new@test.com");
        result.FirstName.Should().Be("First");
        result.LastName.Should().Be("Last");
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
