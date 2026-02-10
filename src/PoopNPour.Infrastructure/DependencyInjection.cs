using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PoopNPour.Abstractions.Authentication;
using PoopNPour.Abstractions.Identity;
using PoopNPour.Application.Authorization;
using PoopNPour.Domain.Common.Auth;
using PoopNPour.Domain.Common.Identity;
using PoopNPour.Infrastructure.Authentication;
using PoopNPour.Infrastructure.Authorization;
using PoopNPour.Infrastructure.Data;
using PoopNPour.Infrastructure.Identity;
using System.Text;

namespace PoopNPour.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        var efSettings = configuration.GetSection("EntityFramework");
        var commandTimeout = efSettings.GetValue<int?>("CommandTimeout") ?? 30;
        var enableSensitiveDataLogging = efSettings.GetValue<bool>("EnableSensitiveDataLogging");
        var enableDetailedErrors = efSettings.GetValue<bool>("EnableDetailedErrors");
        var enableRetryOnFailure = efSettings.GetValue<bool>("EnableRetryOnFailure");
        var maxRetryCount = efSettings.GetValue<int?>("MaxRetryCount") ?? 3;
        var maxRetryDelay = efSettings.GetValue<TimeSpan?>("MaxRetryDelay") ?? TimeSpan.FromSeconds(30);

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.CommandTimeout(commandTimeout);
                
                if (enableRetryOnFailure)
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: maxRetryCount,
                        maxRetryDelay: maxRetryDelay,
                        errorNumbersToAdd: null);
                }
            });

            options.EnableSensitiveDataLogging(enableSensitiveDataLogging);
            options.EnableDetailedErrors(enableDetailedErrors);
            options.EnableServiceProviderCaching(efSettings.GetValue<bool?>("EnableServiceProviderCaching") ?? true);
        });

        // Configure Identity
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            // Password settings
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;

            // User settings
            options.User.RequireUniqueEmail = true;

            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // Sign-in settings
            options.SignIn.RequireConfirmedEmail = true;
            options.SignIn.RequireConfirmedPhoneNumber = false;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // Configure JWT Authentication
        var jwtSettings = configuration.GetSection("Jwt");
        var secretKey = jwtSettings["SecretKey"]
            ?? throw new InvalidOperationException("JWT SecretKey not found in configuration.");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };
        });

        // Add Authorization services
        services.AddAuthorization(options =>
        {
            // Configure all policies using centralized mappings
            PolicyRoleMappings.AddRolesToPolicies(options);
        });

        // Register Current User (for authorization)
        services.AddScoped<IUser, CurrentUser>();

        // Register Identity Service (consolidated interface)
        services.AddScoped<IIdentityService, IdentityService>();

        // Register JWT Token Service
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        // Register AutoMapper
        services.AddAutoMapper(typeof(PoopNPour.Application.Users.Mappings.UserMappingProfile).Assembly);

        // Register Database Seeder
        services.AddScoped<DatabaseSeeder>();

        // Register repositories here as they are created

        return services;
    }
}
