# PoopNPour

A .NET application following Clean Architecture and CQRS patterns.

## Architecture

This project follows **Clean Architecture** and **CQRS (Command Query Responsibility Segregation)** patterns. 

**Please read [ARCHITECTURE.md](./ARCHITECTURE.md) before contributing.**

### Key Principles

- ✅ **Clean Architecture**: Clear separation of concerns with dependency inversion
- ✅ **CQRS**: Separate read (Queries) and write (Commands) operations
- ✅ **MediatR**: Used for implementing CQRS pattern
- ✅ **Feature-based organization**: Code organized by features, not layers

### Project Structure

```
PoopNPour/
├── PoopNPour.Application/          # Application layer (Business logic, CQRS)
│   └── Features/
│       └── {FeatureName}/
│           ├── Queries/            # Read operations
│           ├── Commands/           # Write operations
│           └── Models/             # Domain models/DTOs
│
└── src/
    └── PoopNPour.Api/              # API layer (Presentation)
        ├── Endpoints/              # Feature endpoints
        └── Program.cs              # Configuration only
```

## Getting Started

1. Clone the repository
2. Restore NuGet packages: `dotnet restore`
3. Run the API: `dotnet run --project src/PoopNPour.Api`
4. Navigate to `https://localhost:5001/swagger` (or your configured port)

## Development Guidelines

**All code must follow the patterns defined in [ARCHITECTURE.md](./ARCHITECTURE.md)**

### Quick Reference

- **Queries** (Read): `Application/Features/{Feature}/Queries/`
- **Commands** (Write): `Application/Features/{Feature}/Commands/`
- **Endpoints**: `Api/Endpoints/{Feature}Endpoints.cs`
- **No business logic in API layer**
- **Always use MediatR for operations**

## Technologies

- .NET 10.0
- ASP.NET Core Minimal APIs
- MediatR (CQRS)
- OpenAPI/Swagger
