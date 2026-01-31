# Database Seeding Guide

This project includes automatic database seeding that runs on application startup to ensure initial data is available.

## Overview

The seeding system:
- Creates default roles (Administrator, User, Guest)
- Creates a default admin user (if not exists)
- Runs automatically on application startup
- Uses configuration from `appsettings.json` or `appsettings.Development.json`

## Configuration

### Default Admin User

Configure the default admin user in `appsettings.json`:

```json
{
  "DefaultAdmin": {
    "UserName": "admin",
    "Email": "admin@poopnpour.com",
    "Password": "Admin@123",
    "FirstName": "Admin",
    "LastName": "User"
  }
}
```

**Important Security Notes:**
- ⚠️ **Change the default password in production!**
- ⚠️ The password must meet Identity password requirements:
  - At least 8 characters
  - Contains uppercase letter
  - Contains lowercase letter
  - Contains digit
  - Contains non-alphanumeric character

### Seeding Behavior

- **Idempotent**: Seeding can be run multiple times safely
- **User Check**: Admin user is only created if it doesn't already exist
- **Role Check**: Roles are created if they don't exist
- **Migration**: Database migrations are automatically applied before seeding

## Identity Service

The `IIdentityService` interface provides a clean abstraction for user management operations:

### Available Methods

- `CreateUserAsync()` - Create a new user with password
- `GetUserByUserNameAsync()` - Find user by username
- `GetUserByEmailAsync()` - Find user by email
- `GetUserByIdAsync()` - Find user by ID
- `UserExistsByUserNameAsync()` - Check if username exists
- `UserExistsByEmailAsync()` - Check if email exists
- `AddUserToRoleAsync()` - Add user to a role
- `IsUserInRoleAsync()` - Check if user is in a role
- `EnsureRoleExistsAsync()` - Create role if it doesn't exist

### Usage Example

```csharp
public class MyService
{
    private readonly IIdentityService _identityService;

    public MyService(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task CreateUserAsync()
    {
        var user = await _identityService.CreateUserAsync(
            userName: "john.doe",
            email: "john.doe@example.com",
            password: "SecurePassword123!",
            firstName: "John",
            lastName: "Doe");

        await _identityService.AddUserToRoleAsync(user, Roles.User);
    }
}
```

## Default Roles

The following roles are automatically created:

- **Administrator** - Full system access
- **User** - Standard user access
- **Guest** - Limited access

## Seeding Process

1. **Database Migration**: Ensures database schema is up-to-date
2. **Role Seeding**: Creates default roles if they don't exist
3. **Admin User Seeding**: Creates default admin user if it doesn't exist
4. **Role Assignment**: Assigns Administrator role to default admin user

## Logging

The seeder logs important events:
- When seeding starts/completes
- When admin user is created
- When admin user already exists (skipped)
- Any errors during seeding

Check application logs for seeding status.

## Disabling Seeding

To disable seeding, comment out the seeding code in `Program.cs`:

```csharp
// Seed database
// using (var scope = app.Services.CreateScope())
// {
//     var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
//     await seeder.SeedAsync();
// }
```

## Custom Seeding

To add custom seeding logic, extend the `DatabaseSeeder` class:

```csharp
public class DatabaseSeeder
{
    // ... existing code ...

    private async Task SeedCustomDataAsync(CancellationToken cancellationToken)
    {
        // Your custom seeding logic here
    }
}
```

Then call it from `SeedAsync()`:

```csharp
public async Task SeedAsync(CancellationToken cancellationToken = default)
{
    // ... existing seeding ...
    await SeedCustomDataAsync(cancellationToken);
}
```

## Troubleshooting

### Admin User Not Created

- Check `appsettings.json` has `DefaultAdmin` section configured
- Verify password meets requirements (8+ chars, uppercase, lowercase, digit, special char)
- Check application logs for errors
- Verify database connection is working

### Roles Not Created

- Check application logs for errors
- Verify `Roles` constants are defined in `PoopNPour.Domain.Common.Auth.Roles`
- Ensure database connection is working

### Seeding Runs Multiple Times

This is expected behavior. The seeder is idempotent and checks for existing data before creating.
