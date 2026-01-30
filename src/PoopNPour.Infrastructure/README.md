# Infrastructure Layer

This layer contains all external concerns including database access, external services, and infrastructure implementations.

## Identity & Authentication

ASP.NET Core Identity is configured with JWT Bearer authentication for API usage.

### Configuration

Identity is configured in `DependencyInjection.cs` with:
- Password requirements (8+ characters, uppercase, lowercase, digit, special character)
- Unique email requirement
- Account lockout after 5 failed attempts
- JWT token authentication

### JWT Settings

Configure JWT settings in `appsettings.json`:

```json
{
  "Jwt": {
    "SecretKey": "YourSecretKeyHere_MustBeAtLeast32CharactersLong",
    "Issuer": "PoopNPour",
    "Audience": "PoopNPour",
    "ExpirationInMinutes": 60
  }
}
```

**Important:** Change the `SecretKey` in production! Use a strong, randomly generated key.

## Database Setup

### Prerequisites

- SQL Server installed and running (LocalDB, SQL Server Express, or full SQL Server)
- .NET EF Core Tools installed: `dotnet tool install --global dotnet-ef`

### Connection String

Connection strings are stored in `database.config.json` (not in version control). 

1. Copy the example file:
   ```bash
   cp src/PoopNPour.Api/database.config.json.example src/PoopNPour.Api/database.config.json
   ```

2. Update `database.config.json` with your connection string:

**SQL Server:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PoopNPourDb;User Id=sa;Password=your_password;TrustServerCertificate=true;"
  }
}
```

**SQL Server LocalDB:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PoopNPourDb;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

**Note:** `database.config.json` is gitignored to prevent committing sensitive credentials.

### Entity Framework Configuration

Entity Framework settings are configured in `appsettings.json`:

```json
{
  "EntityFramework": {
    "CommandTimeout": 30,
    "EnableSensitiveDataLogging": false,
    "EnableDetailedErrors": false,
    "EnableServiceProviderCaching": true,
    "EnableRetryOnFailure": true,
    "MaxRetryCount": 3,
    "MaxRetryDelay": "00:00:30"
  }
}
```

**Settings:**
- `CommandTimeout`: Command timeout in seconds (default: 30)
- `EnableSensitiveDataLogging`: Log parameter values in SQL queries (disabled in production)
- `EnableDetailedErrors`: Include detailed error information (disabled in production)
- `EnableServiceProviderCaching`: Cache EF Core service provider (recommended: true)
- `EnableRetryOnFailure`: Enable automatic retry on transient failures
- `MaxRetryCount`: Maximum number of retry attempts (default: 3)
- `MaxRetryDelay`: Maximum delay between retries (format: HH:MM:SS)

**Note:** In Development, `EnableSensitiveDataLogging` and `EnableDetailedErrors` are enabled for easier debugging.

### Creating Migrations

To create a new migration:

```bash
dotnet ef migrations add MigrationName --project src/PoopNPour.Infrastructure --startup-project src/PoopNPour.Api
```

### Applying Migrations

To apply migrations to the database:

```bash
dotnet ef database update --project src/PoopNPour.Infrastructure --startup-project src/PoopNPour.Api
```

### Project Structure

```
PoopNPour.Infrastructure/
  Data/
    ApplicationDbContext.cs          # EF Core DbContext (uses Domain entities)
    Configurations/                  # EF Core entity configurations
      WeatherForecastConfiguration.cs
  Repositories/                      # Repository implementations
    WeatherForecastRepository.cs
  DependencyInjection.cs           # DI configuration
```

## Clean Architecture Principles

- ✅ Infrastructure implements interfaces defined in Application layer
- ✅ Infrastructure references Domain layer for entities
- ✅ Infrastructure references Application layer for interfaces
- ✅ No direct dependencies from Application to Infrastructure
- ✅ All database access goes through repositories
- ✅ DbContext uses Domain entities (not separate Infrastructure entities)
- ✅ Repository implementations map Domain entities ↔ Application DTOs
- ✅ DbContext is scoped per request
