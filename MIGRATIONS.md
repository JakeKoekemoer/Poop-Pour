# Entity Framework Migrations Guide

## Prerequisites

1. **EF Core Tools** - Install globally (if not already installed):
   ```powershell
   dotnet tool install --global dotnet-ef
   ```

2. **Database Configuration** - Ensure `database.config.json` exists with your connection string:
   ```bash
   cp src/PoopNPour.Api/database.config.json.example src/PoopNPour.Api/database.config.json
   ```
   Then update it with your actual connection string.

## Using Visual Studio Package Manager Console

### Setting Up

1. Open **Package Manager Console** in Visual Studio (Tools → NuGet Package Manager → Package Manager Console)

2. Set the **Default Project** to `PoopNPour.Infrastructure`

3. Set the **Default Project** dropdown (if available) or use the `-StartupProject` parameter

### Creating Migrations

**PowerShell Command (Package Manager Console):**
```powershell
Add-Migration MigrationName -StartupProject src\PoopNPour.Api -Project src\PoopNPour.Infrastructure
```

**Example:**
```powershell
Add-Migration InitialCreate -StartupProject src\PoopNPour.Api -Project src\PoopNPour.Infrastructure
```

### Applying Migrations

**PowerShell Command (Package Manager Console):**
```powershell
Update-Database -StartupProject src\PoopNPour.Api -Project src\PoopNPour.Infrastructure
```

## Using .NET CLI (Command Line)

### Creating Migrations

```bash
dotnet ef migrations add MigrationName --project src/PoopNPour.Infrastructure --startup-project src/PoopNPour.Api
```

**Example:**
```bash
dotnet ef migrations add InitialCreate --project src/PoopNPour.Infrastructure --startup-project src/PoopNPour.Api
```

### Applying Migrations

```bash
dotnet ef database update --project src/PoopNPour.Infrastructure --startup-project src/PoopNPour.Api
```

### Listing Migrations

```bash
dotnet ef migrations list --project src/PoopNPour.Infrastructure --startup-project src/PoopNPour.Api
```

### Removing Last Migration

```bash
dotnet ef migrations remove --project src/PoopNPour.Infrastructure --startup-project src/PoopNPour.Api
```

## Troubleshooting

### Issue: "add-migration is not recognized"

**Solution:** Use `Add-Migration` (PowerShell cmdlet) in Package Manager Console, or use `dotnet ef` commands in terminal.

### Issue: "Unable to create an object of type 'ApplicationDbContext'"

**Solution:** Ensure:
- `database.config.json` exists and has a valid connection string
- The startup project (`PoopNPour.Api`) can access the configuration
- All required packages are restored: `dotnet restore`

### Issue: Package Manager Console doesn't recognize EF commands

**Solution:** 
1. Ensure `Microsoft.EntityFrameworkCore.Design` is installed in the Infrastructure project (it is)
2. Restore packages: `dotnet restore`
3. Try using `.NET CLI` commands instead: `dotnet ef migrations add ...`

## Migration Files Location

Migrations are created in:
```
src/PoopNPour.Infrastructure/Migrations/
```

**Note:** Migration files are automatically generated and should be committed to version control.
